using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;

namespace CineBuzzApi.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private CartService _service;
        private CineBuzzDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new CartService(_context);
        }


        [TestMethod]
        public async Task GetCartAsync_CreatesNewCart_WhenCartDoesNotExist()
        {
            // Arrange
            var cartId = 2;

            // Act
            var result = await _service.GetCartAsync(cartId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(0, result.Items.Count);
            Assert.AreEqual(0, result.Total);
        }

        [TestMethod]
        public async Task GetCartAsync_ReturnsExistingCart_WhenCartExists()
        {
            // Arrange
            var cartId = 2;
            var expectedCart = new Cart { CartId = cartId, Total = 100, UserId = 1, Items = new List<CartItem>() };
            _context.Carts.Add(expectedCart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetCartAsync(cartId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCart.CartId, result.CartId);
            Assert.AreEqual(expectedCart.Total, result.Total);
            Assert.AreEqual(expectedCart.UserId, result.UserId);
        }
        [TestMethod]
        public async Task AddTicketToCartAsync_AddsTicket_WhenCartAndTicketExist()
        {
            // Arrange
            var cartId = 1; // This cart is already seeded in the database
            var ticketId = 2;
            var quantity = 2;

            // Access the existing seeded cart using GetCartAsync
            var existingCart = await _service.GetCartAsync(cartId);

            // Act
            var result = await _service.AddTicketToCartAsync(cartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(2, result.Items.Count);

            // Check if the ticket exists after adding
            var addedTicket = result.Items.FirstOrDefault(i => i.TicketId == ticketId);
            Assert.IsNotNull(addedTicket);
            Assert.AreEqual(ticketId, addedTicket.TicketId);
            Assert.AreEqual(quantity, addedTicket.Quantity);
        }
        [TestMethod]
        public async Task RemoveTicketFromCartAsync_RemovesTicket_WhenTicketExistsInCart()
        {
            // Arrange
            var cartId = 1;
            var ticketId = 1; // Ticket with ID 1 already exists in the cart
            var existingCart = await _service.GetCartAsync(cartId);

            // Act
            var result = await _service.RemoveTicketFromCartAsync(cartId, ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(0, result.Items.Count); // Should have no tickets after removal
        }
    }
}
