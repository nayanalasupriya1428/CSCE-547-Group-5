using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;  // Provides Entity Framework support.
using Microsoft.EntityFrameworkCore.InMemory;  // For in-memory database testing.
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;  // Built-in .NET library for JSON
using CineBuzzApi.Services;
using CineBuzzApi.Controllers;
using CineBuzzApi.Models;


namespace CineBuzzApi.Tests
{
    [TestClass]
    public class CartsControllerTests
    {
        private Mock<ICartService> _mockCartService;
        private CartsController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockCartService = new Mock<ICartService>();
            _controller = new CartsController(_mockCartService.Object);
        }

        [TestMethod]
        public async Task GetCart_ValidId_ReturnsCart()
        {
            // Arrange
            var expectedCart = new Cart { CartId = 1, Total = 100 };
            _mockCartService.Setup(s => s.GetCartAsync(1)).ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.GetCart(1);
            var okResult = result.Result as OkObjectResult;


            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedCart = okResult.Value as Cart;
            Assert.IsNotNull(returnedCart);
            Assert.AreEqual(expectedCart.CartId, returnedCart.CartId);
            Assert.AreEqual(expectedCart.Total, returnedCart.Total);
        }

        [TestMethod]
        public async Task GetCart_NullId_ReturnsNewCart()
        {
            // Arrange
            var newCart = new Cart { CartId = 2, Total = 0 };
            _mockCartService.Setup(s => s.GetCartAsync(null)).ReturnsAsync(newCart);

            // Act
            var result = await _controller.GetCart(null);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedCart = okResult.Value as Cart;
            Assert.IsNotNull(returnedCart);
            Assert.AreEqual(newCart.CartId, returnedCart.CartId);
            Assert.AreEqual(newCart.Total, returnedCart.Total);
        }
        [TestMethod]
        public async Task GetCart_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockCartService.Setup(s => s.GetCartAsync(It.IsAny<int>())).ReturnsAsync((Cart)null);

            // Act
            var result = await _controller.GetCart(999);
            var notFoundResult = result.Result as NotFoundResult;

            // Assert
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

    }

}