// @author Scott Do (Reshlynt)
// Unit tests for CartController class in MovieReviewApi.
// @date 2024-11-5
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Controllers;
using MovieReviewApi.Models;

namespace MovieReviewApi.Tests
{
    [TestClass]
    public class PaymentControllerTests
    {
        private MovieContext _context;
        private PaymentController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Set up the In-Memory Database for MovieContext
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString())
                .Options;

            _context = new MovieContext(options);
            SeedTestData(_context);
            _controller = new PaymentController(_context);
        }

        /// <summary>
        /// Seeds the in-memory database with initial data for testing purposes.
        /// </summary>
        /// <param name="context">The in-memory movie context to be seeded with test data.</param>
        private void SeedTestData(MovieContext context)
        {
            var movie1 = new Movie { Id = 1, MovieTitle = "Movie 1", Genre = "Action", Rating = 4 };
            var movie2 = new Movie { Id = 2, MovieTitle = "Movie 2", Genre = "Comedy", Rating = 3 };

            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieId = 1, EventName = "Movie 1 Screening", Price = 15.00M, Movie = movie1 },
                new Ticket { TicketId = 2, MovieId = 2, EventName = "Movie 2 Matinee", Price = 10.00M, Movie = movie2 }
            };

            var cart = new Cart
            {
                CartId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemId = 1, CartId = 1, TicketId = 1, Quantity = 2, Ticket = tickets[0] },
                    new CartItem { CartItemId = 2, CartId = 1, TicketId = 2, Quantity = 1, Ticket = tickets[1] }
                }
            };

            context.Movie.AddRange(movie1, movie2);
            context.Tickets.AddRange(tickets);
            context.Carts.Add(cart);
            context.SaveChanges();
        }

        /// <summary>
        /// Tests that a valid payment request returns a successful result.
        /// </summary>
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnSuccess_WhenPaymentIsValid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1,
                CardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(6).ToString("MM/yyyy"),
                CardholderName = "John Doe",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var responseValue = okResult.Value;
            var messageProperty = responseValue.GetType().GetProperty("Message");
            var message = messageProperty?.GetValue(responseValue) as string;
            Assert.AreEqual("Payment successful!", message);

            var totalAmountProperty = responseValue.GetType().GetProperty("TotalAmount");
            var totalAmount = totalAmountProperty?.GetValue(responseValue) as decimal?;
            Assert.AreEqual(40.00M, totalAmount);
        }

        /// <summary>
        /// Tests that attempting to process payment for a non-existent cart returns a BadRequest result.
        /// </summary>
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenCartDoesNotExist()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 999,
                CardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(6).ToString("MM/yyyy"),
                CardholderName = "John Doe",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Cart is empty or does not exist.", badRequestResult.Value);
        }

        /// <summary>
        /// Tests that an invalid card number returns a BadRequest result.
        /// </summary>
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenCardNumInvalid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1,
                CardNumber = " ",
                ExpirationDate = DateTime.Now.AddMonths(6).ToString("MM/yyyy"),
                CardholderName = "John Doe",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid card number.", badRequestResult.Value);
        }

        /// <summary>
        /// Tests that an expired card returns a BadRequest result.
        /// </summary>
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenCardInvalid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1,
                CardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(-1).ToString("MM/yyyy"),
                CardholderName = "John Doe",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid or expired card.", badRequestResult.Value);
        }

        /// <summary>
        /// Tests that an invalid cardholder name returns a BadRequest result.
        /// </summary>
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenNameInvalid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1,
                CardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(6).ToString("MM/yyyy"),
                CardholderName = " ",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Cardholder name is required.", badRequestResult.Value);
        }

        /// <summary>
        /// Tests that an invalid CVC returns a BadRequest result.
        /// </summary>
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenCVCInvalid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1,
                CardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(6).ToString("MM/yyyy"),
                CardholderName = "John Doe",
                CVC = "1"
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid CVC.", badRequestResult.Value);
        }
    }
}
