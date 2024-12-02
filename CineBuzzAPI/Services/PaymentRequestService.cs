// Required namespaces for database context, model access, and asynchronous programming
using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // The PaymentRequestService class implements the IPaymentRequestService interface.
    public class PaymentRequestService : IPaymentRequestService
    {
        // Private field to hold the database context.
        private readonly CineBuzzDbContext _context;

        // Constructor that accepts a database context and uses it to perform data operations.
        public PaymentRequestService(CineBuzzDbContext context)
        {
            _context = context;
        }

        // Retrieves all payment requests from the database asynchronously.
        public async Task<IEnumerable<PaymentRequest>> GetAllPaymentRequestsAsync()
        {
            return await _context.PaymentRequests.ToListAsync();
        }

        // Retrieves a single payment request by its ID asynchronously.
        public async Task<PaymentRequest?> GetPaymentRequestByIdAsync(int paymentRequestId)
        {
            return await _context.PaymentRequests.FindAsync(paymentRequestId);
        }

        // Adds a new payment request to the database asynchronously and saves the changes.
        public async Task<PaymentRequest> AddPaymentRequestAsync(PaymentRequest paymentRequest)
        {
            _context.PaymentRequests.Add(paymentRequest);
            await _context.SaveChangesAsync();
            return paymentRequest;
        }

        // Updates an existing payment request with new details asynchronously if it exists, otherwise returns null.
        public async Task<PaymentRequest?> UpdatePaymentRequestAsync(int paymentRequestId, PaymentRequest paymentRequest)
        {
            var existingRequest = await _context.PaymentRequests.FindAsync(paymentRequestId);
            if (existingRequest == null) return null;

            // Update properties of the existing payment request
            existingRequest.CardNumber = paymentRequest.CardNumber;
            existingRequest.ExpirationDate = paymentRequest.ExpirationDate;
            existingRequest.CardholderName = paymentRequest.CardholderName;
            existingRequest.CVC = paymentRequest.CVC;
            existingRequest.CartId = paymentRequest.CartId;

            // Save the updated context to the database
            await _context.SaveChangesAsync();
            return existingRequest;
        }

        // Deletes a payment request from the database asynchronously if it exists.
        public async Task DeletePaymentRequestAsync(int paymentRequestId)
        {
            var request = await _context.PaymentRequests.FindAsync(paymentRequestId);
            if (request != null)
            {
                _context.PaymentRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}
