using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;

namespace CineBuzzApi.Tests
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
                .UseInMemoryDatabase(databaseName: "CineBuzzDb")
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
            var cartId = 2;
            var ticketId = 5;
            var quantity = 3;
            var existingCart = new Cart { CartId = cartId, Items = new List<CartItem>() };
            var ticket = new Ticket { TicketId = ticketId, Price = 50 };

            _context.Carts.Add(existingCart);
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.AddTicketToCartAsync(cartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingCart.CartId, result.CartId);
            Assert.AreEqual(1, existingCart.Items.Count);
            Assert.AreEqual(ticketId, existingCart.Items[0].TicketId);
            Assert.AreEqual(quantity, existingCart.Items[0].Quantity);
            Assert.AreEqual(150, result.Total); // Assuming Total is updated accordingly
        }
    }
}
