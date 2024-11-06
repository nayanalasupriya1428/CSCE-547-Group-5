using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            // 1. Set up the In-Memory Database for MovieContext
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString()) // Unique database for each test
                .Options;

            _context = new MovieContext(options);

            // 2. Seed the Database with Initial Data
            SeedTestData(_context);

            // 3. Set up the PaymentController with the MovieContext
            _controller = new PaymentController(_context);
        }

        // Helper method to seed initial data into the in-memory database
        private void SeedTestData(MovieContext context)
        {
            // Create movie data
            var movie1 = new Movie { Id = 1, MovieTitle = "Movie 1", Genre = "Action", Rating = 4 };
            var movie2 = new Movie { Id = 2, MovieTitle = "Movie 2", Genre = "Comedy", Rating = 3 };

            // Create ticket data linked to movies
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieId = 1, EventName = "Movie 1 Screening", Price = 15.00M, Movie = movie1 },
                new Ticket { TicketId = 2, MovieId = 2, EventName = "Movie 2 Matinee", Price = 10.00M, Movie = movie2 }
            };

            // Create cart data with some tickets
            var cart = new Cart
            {
                CartId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemId = 1, CartId = 1, TicketId = 1, Quantity = 2, Ticket = tickets[0] },
                    new CartItem { CartItemId = 2, CartId = 1, TicketId = 2, Quantity = 1, Ticket = tickets[1] }
                }
            };

            // Add movies, tickets, and cart to the context
            context.Movie.AddRange(movie1, movie2);
            context.Tickets.AddRange(tickets);
            context.Carts.Add(cart);
            context.SaveChanges();
        }

        [TestMethod]
        public async Task ProcessPayment_ShouldReturnSuccess_WhenPaymentIsValid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1, // Cart with ID 1 exists in the seeded data
                CardNumber = "1234567812345678", // Valid card number (16 digits)
                ExpirationDate = DateTime.Now.AddMonths(6).ToString("MM/yyyy"), // Valid expiration date in the future
                CardholderName = "John Doe", // Valid cardholder name
                CVC = "123" // Valid CVC (3 digits)
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            // Ensure the result is not null
            Assert.IsNotNull(result);

            // Verify that the result is of type OkObjectResult (indicating success)
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            // Cast the result to OkObjectResult to access its properties
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult); // Ensure cast is successful

            // Access the response content
            dynamic response = okResult.Value;
            Assert.AreEqual("Payment successful!", response.Message);
            Assert.AreEqual(40.00M, response.TotalAmount); // The total amount for cart ID 1 should be 40.00
        }

        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenCartDoesNotExist()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 999, // Non-existent cart ID
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
        [TestMethod]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenCardInvalid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CartId = 1,
                CardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(-1).ToString("MM/yyyy"), // Expired date (in the past)
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
