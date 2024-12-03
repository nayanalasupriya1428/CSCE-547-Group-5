using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CineBuzzApi.Services
{
    [TestClass]
    public class PaymentRequestServiceTests
    {
        private PaymentRequestService _service;
        private CineBuzzDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new PaymentRequestService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetAllPaymentRequestsAsync_ReturnsAllPaymentRequests_WhenRequestsExist()
        {
            // Arrange - there are 4 payment requests
            _context.PaymentRequests.RemoveRange(_context.PaymentRequests);
            var paymentRequests = new List<PaymentRequest>
            {
                new PaymentRequest { PaymentRequestId = 3, CardNumber = "4111111111111111", ExpirationDate = "12/25", CardholderName = "John Doe", CVC = "123", CartId = 1 },
                new PaymentRequest { PaymentRequestId = 4, CardNumber = "5555555555554444", ExpirationDate = "11/24", CardholderName = "Jane Smith", CVC = "456", CartId = 2 }
            };
            _context.PaymentRequests.AddRange(paymentRequests);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllPaymentRequestsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        [TestMethod]
        public async Task AddPaymentRequestAsync_AddsPaymentRequestSuccessfully_WhenRequestIsValid()
        {
            // Arrange
            _context.PaymentRequests.RemoveRange(_context.PaymentRequests);
            var newPaymentRequest = new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpirationDate = "10/26",
                CardholderName = "Alice Wonderland",
                CVC = "789",
                CartId = 3
            };

            // Act
            var result = await _service.AddPaymentRequestAsync(newPaymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("4111111111111111", result.CardNumber);
            Assert.AreEqual("10/26", result.ExpirationDate);
            Assert.AreEqual("Alice Wonderland", result.CardholderName);
            Assert.AreEqual("789", result.CVC);
            Assert.AreEqual(1, await _context.PaymentRequests.CountAsync()); // Ensure that there is only one payment request in the database
        }
        [TestMethod]
        public async Task UpdatePaymentRequestAsync_UpdatesPaymentRequestSuccessfully_WhenRequestExists()
        {
            // Arrange
            _context.PaymentRequests.RemoveRange(_context.PaymentRequests);
            var existingPaymentRequest = new PaymentRequest
            {
                PaymentRequestId = 1,
                CardNumber = "4111111111111111",
                ExpirationDate = "12/25",
                CardholderName = "John Doe",
                CVC = "123",
                CartId = 1
            };

            _context.PaymentRequests.Add(existingPaymentRequest);
            await _context.SaveChangesAsync();

            var updatedPaymentRequest = new PaymentRequest
            {
                CardNumber = "5555555555554444",
                ExpirationDate = "11/24",
                CardholderName = "Jane Doe",
                CVC = "456",
                CartId = 2
            };

            // Act
            var result = await _service.UpdatePaymentRequestAsync(existingPaymentRequest.PaymentRequestId, updatedPaymentRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("5555555555554444", result.CardNumber);
            Assert.AreEqual("11/24", result.ExpirationDate);
            Assert.AreEqual("Jane Doe", result.CardholderName);
            Assert.AreEqual("456", result.CVC);
            Assert.AreEqual(2, result.CartId);
        }
        [TestMethod]
        public async Task DeletePaymentRequestAsync_DeletesPaymentRequestSuccessfully_WhenRequestExists()
        {
            // Arrange
            _context.PaymentRequests.RemoveRange(_context.PaymentRequests);
            var paymentRequest = new PaymentRequest
            {
                PaymentRequestId = 1,
                CardNumber = "4111111111111111",
                ExpirationDate = "12/25",
                CardholderName = "John Doe",
                CVC = "123",
                CartId = 1
            };

            _context.PaymentRequests.Add(paymentRequest);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeletePaymentRequestAsync(paymentRequest.PaymentRequestId);

            // Assert
            var deletedRequest = await _context.PaymentRequests.FindAsync(paymentRequest.PaymentRequestId);
            Assert.IsNull(deletedRequest); // Verify that the payment request is deleted
            Assert.AreEqual(0, await _context.PaymentRequests.CountAsync()); // Ensure no payment requests remain
        }

    }
}
