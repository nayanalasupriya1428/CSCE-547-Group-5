using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CineBuzzTests.Controllers
{
    [TestClass]
    public class PaymentRequestsControllerTests
    {
        private Mock<IPaymentRequestService> _mockPaymentRequestService;
        private PaymentRequestsController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the mock service
            _mockPaymentRequestService = new Mock<IPaymentRequestService>();

            // Instantiate the controller with the mock service
            _controller = new PaymentRequestsController(_mockPaymentRequestService.Object);
        }


        [TestMethod]
        public async Task ProcessPayment_ValidRequest_ReturnsOk()
        {
            // Arrange
            var validRequest = new PaymentRequest
            {
                CartId = 123,
                PaymentRequestId = 1,
                CardNumber = "4111111111111111",
                ExpirationDate = "12/25",
                CardholderName = "John Doe",
                CVC = "123"
            };

            _mockPaymentRequestService
                .Setup(service => service.AddPaymentRequestAsync(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(validRequest);

            // Act
            var result = await _controller.ProcessPayment(validRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Payment processed successfully", okResult.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }
        [TestMethod]
        public async Task ProcessPayment_InvalidCardNumber_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new PaymentRequest
            {
                CartId = 123,
                PaymentRequestId = 1,
                CardNumber = "abcd1234",
                ExpirationDate = "12/25",
                CardholderName = "John Doe",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(invalidRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid card number", badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value));
        }
        [TestMethod]
        public async Task ProcessPayment_InvalidExpirationDate_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new PaymentRequest
            {
                CartId = 123,
                PaymentRequestId = 1,
                CardNumber = "4111111111111111",
                ExpirationDate = "13/25",
                CardholderName = "John Doe",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(invalidRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid expiration date format", badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value));
        }
        [TestMethod]
        public async Task ProcessPayment_MissingCardholderName_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new PaymentRequest
            {
                CartId = 123,
                PaymentRequestId = 1,
                CardNumber = "4111111111111111",
                ExpirationDate = "12/25",
                CardholderName = "",
                CVC = "123"
            };

            // Act
            var result = await _controller.ProcessPayment(invalidRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Cardholder name is required", badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value));
        }
        [TestMethod]
        public async Task ProcessPayment_InvalidCVC_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new PaymentRequest
            {
                CartId = 123,
                PaymentRequestId = 1,
                CardNumber = "4111111111111111",
                ExpirationDate = "12/25",
                CardholderName = "John Doe",
                CVC = "12"
            };

            // Act
            var result = await _controller.ProcessPayment(invalidRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid CVC", badRequestResult.Value.GetType().GetProperty("Message")?.GetValue(badRequestResult.Value));
        }
    }
}
