using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CineBuzzApi.Controllers;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CineBuzzTests.Controllers
{
    [TestClass]
    public class CartsControllerTests
    {
        // Mocked CartService to isolate the controller for testing
        private readonly Mock<ICartService> _mockCartService;
        private readonly CartsController _controller;

        // Constructor to set up the test class with necessary components
        public CartsControllerTests()
        {
            // Instantiate the mock service
            _mockCartService = new Mock<ICartService>();

            // Pass the mocked service to the CartsController
            _controller = new CartsController(_mockCartService.Object);
        }

        [TestMethod]
        public async Task GetCart_ReturnsOkResult_WithCart()
        {
            // Arrange
            var cartId = 2;
            var expectedCart = new Cart { CartId = cartId, Total = 100, UserId = 1, Items = new List<CartItem>() };
            _mockCartService.Setup(service => service.GetCartAsync(cartId)).ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.GetCart(cartId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(Cart));
            var cart = okResult.Value as Cart;
            Assert.AreEqual(expectedCart.CartId, cart.CartId);
            Assert.AreEqual(expectedCart.Total, cart.Total);
            Assert.AreEqual(expectedCart.UserId, cart.UserId);
        }

        [TestMethod]
        public async Task AddTicketToCart_ReturnsOkResult_WithUpdatedCart()
        {
            // Arrange
            var cartId = 2;
            var ticketId = 5;
            var quantity = 3;
            var expectedCart = new Cart { CartId = cartId, Total = 150, UserId = 1, Items = new List<CartItem> { new CartItem { CartItemId = 1, CartId = cartId, TicketId = ticketId, Quantity = quantity } } };
            _mockCartService.Setup(service => service.AddTicketToCartAsync(cartId, ticketId, quantity)).ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.AddTicketToCart(cartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(Cart));
            var cart = okResult.Value as Cart;
            Assert.AreEqual(expectedCart.CartId, cart.CartId);
            Assert.AreEqual(expectedCart.Total, cart.Total);
            Assert.AreEqual(expectedCart.Items.Count, cart.Items.Count);
            Assert.AreEqual(expectedCart.Items[0].TicketId, cart.Items[0].TicketId);
            Assert.AreEqual(expectedCart.Items[0].Quantity, cart.Items[0].Quantity);
        }
        [TestMethod]
        public async Task RemoveTicketFromCart_ReturnsOkResult_WithUpdatedCart()
        {
            // Arrange
            var cartId = 2;
            var ticketId = 5;
            var expectedCart = new Cart { CartId = cartId, Total = 100, UserId = 1, Items = new List<CartItem>() }; // Expected cart after ticket removal
            _mockCartService.Setup(service => service.RemoveTicketFromCartAsync(cartId, ticketId)).ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.RemoveTicketFromCart(cartId, ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(Cart));
            var cart = okResult.Value as Cart;
            Assert.AreEqual(expectedCart.CartId, cart.CartId);
            Assert.AreEqual(expectedCart.Total, cart.Total);
            Assert.AreEqual(expectedCart.Items.Count, cart.Items.Count);
        }
    }
}