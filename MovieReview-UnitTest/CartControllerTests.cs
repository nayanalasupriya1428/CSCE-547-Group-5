using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieReviewApi.Models;

namespace MovieReviewApi.Tests
{
    [TestClass]
    public class CartControllerTests
    {
        private MovieContext _context;
        private CartController _controller;

        [TestInitialize]
        public void Setup()
        {
            // 1. Set up In-Memory Database for MovieContext
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString()) // Use unique DB for each test
                .Options;

            _context = new MovieContext(options);

            // 2. Seed the Database with Initial Data (if needed)
            SeedTestData(_context);

            // 3. Set up the Controller with the Context
            _controller = new CartController(_context);
        }

        // Helper method to seed data into the in-memory database
        private void SeedTestData(MovieContext context)
        {
            var cart = new Cart
            {
                CartId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemId = 1, CartId = 1, TicketId = 1, Quantity = 2 },
                    new CartItem { CartItemId = 2, CartId = 1, TicketId = 2, Quantity = 1 }
                }
            };

            var ticket1 = new Ticket
            {
                TicketId = 1,
                EventName = "Movie 1",
                Price = 15.00M
            };

            var ticket2 = new Ticket
            {
                TicketId = 2,
                EventName = "Movie 2",
                Price = 10.00M
            };

            context.Carts.Add(cart);
            context.Tickets.AddRange(ticket1, ticket2);
            context.SaveChanges();
        }

        [TestMethod]
        public async Task AddTicketToCart_ShouldAddTicketSuccessfully()
        {
            // Arrange
            int cartId = 1;
            int ticketId = 1;
            int quantity = 1;

            // Act
            var result = await _controller.AddTicketToCart(cartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as dynamic;
            Assert.AreEqual("Ticket added to cart successfully.", response.Message);

            // Verify ticket is added to the cart
            var updatedCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
            Assert.IsNotNull(updatedCart);
            var cartItem = updatedCart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
            Assert.IsNotNull(cartItem);
            Assert.AreEqual(3, cartItem.Quantity);
        }

        [TestMethod]
        public async Task AddTicketToCart_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            // Arrange
            int nonExistentCartId = 999;
            int ticketId = 1;
            int quantity = 1;

            // Act
            var result = await _controller.AddTicketToCart(nonExistentCartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Cart not found.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task AddTicketToCart_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int cartId = 1;
            int nonExistentTicketId = 999;
            int quantity = 1;

            // Act
            var result = await _controller.AddTicketToCart(cartId, nonExistentTicketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Ticket not found.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task RemoveTicketFromCart_ShouldRemoveTicketSuccessfully()
        {
            // Arrange
            int cartId = 1;          // The cart ID that exists (from seed data)
            int ticketId = 1;        // The ticket ID that exists in the cart (from seed data)

            // Act
            var result = await _controller.RemoveTicketFromCart(cartId, ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as dynamic;
            Assert.IsNotNull(response);

            // Verify that the ticket was removed from the cart
            var updatedCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
            Assert.IsNotNull(updatedCart);

            var removedItem = updatedCart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
            Assert.IsNull(removedItem);

            // Verify the total amount after removing the ticket
            var totalAmount = updatedCart.CartItems.Sum(ci => ci.Quantity * ci.Ticket.Price);
            Assert.AreEqual(response.Total, totalAmount);
        }

        [TestMethod]
        public async Task RemoveTicketFromCart_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            // Arrange
            int nonExistentCartId = 999;  // Cart ID that does not exist
            int ticketId = 1;

            // Act
            var result = await _controller.RemoveTicketFromCart(nonExistentCartId, ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Cart not found.", notFoundResult.Value);
        }
    }
}