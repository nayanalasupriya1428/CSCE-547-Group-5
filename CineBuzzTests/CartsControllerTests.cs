using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace CineBuzzTests
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

    }

}