/*
 * Unit tests for the Cart Service layer. Verify the functionality of the CartService class.
 * @author Scott Do (Reshlynt)
 */
using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;

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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Creates a unique in-memory database for each test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new CartService(_context);
        }

        /// <summary>
        /// Tests if a new cart is created when the specified cart ID does not exist.
        /// </summary>
        [TestMethod]
        public async Task GetCartAsync_CreatesNewCart_WhenCartDoesNotExist()
        {
            var cartId = 2;
            var result = await _service.GetCartAsync(cartId);

            Assert.IsNotNull(result);
            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(0, result.Items.Count);
            Assert.AreEqual(0, result.Total);
        }

        /// <summary>
        /// Tests if an existing cart is returned when the specified cart ID exists.
        /// </summary>
        [TestMethod]
        public async Task GetCartAsync_ReturnsExistingCart_WhenCartExists()
        {
            var cartId = 2;
            var expectedCart = new Cart { CartId = cartId, Total = 100, UserId = 1, Items = new List<CartItem>() };
            _context.Carts.Add(expectedCart);
            await _context.SaveChangesAsync();

            var result = await _service.GetCartAsync(cartId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCart.CartId, result.CartId);
            Assert.AreEqual(expectedCart.Total, result.Total);
            Assert.AreEqual(expectedCart.UserId, result.UserId);
        }

        /// <summary>
        /// Tests if a ticket is added to the cart when both the cart and ticket exist.
        /// </summary>
        [TestMethod]
        public async Task AddTicketToCartAsync_AddsTicket_WhenCartAndTicketExist()
        {
            var cartId = 1;
            var ticketId = 2;
            var quantity = 2;

            var existingCart = await _service.GetCartAsync(cartId);

            var result = await _service.AddTicketToCartAsync(cartId, ticketId, quantity);

            Assert.IsNotNull(result);
            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(2, result.Items.Count);

            var addedTicket = result.Items.FirstOrDefault(i => i.TicketId == ticketId);
            Assert.IsNotNull(addedTicket);
            Assert.AreEqual(ticketId, addedTicket.TicketId);
            Assert.AreEqual(quantity, addedTicket.Quantity);
        }

        /// <summary>
        /// Tests if a ticket is removed from the cart when the ticket exists in the cart.
        /// </summary>
        [TestMethod]
        public async Task RemoveTicketFromCartAsync_RemovesTicket_WhenTicketExistsInCart()
        {
            var cartId = 1;
            var ticketId = 1;
            var existingCart = await _service.GetCartAsync(cartId);

            var result = await _service.RemoveTicketFromCartAsync(cartId, ticketId);

            Assert.IsNotNull(result);
            Assert.AreEqual(cartId, result.CartId);
            Assert.AreEqual(0, result.Items.Count);
        }
    }
}
