using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly CineBuzzDbContext _context;

        public PaymentRequestService(CineBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentRequest>> GetAllPaymentRequestsAsync()
        {
            return await _context.PaymentRequests.ToListAsync();
        }

        public async Task<PaymentRequest?> GetPaymentRequestByIdAsync(int paymentRequestId)
        {
            return await _context.PaymentRequests.FindAsync(paymentRequestId);
        }

        public async Task<PaymentRequest> AddPaymentRequestAsync(PaymentRequest paymentRequest)
        {
            _context.PaymentRequests.Add(paymentRequest);
            await _context.SaveChangesAsync();
            return paymentRequest;
        }

        public async Task<PaymentRequest?> UpdatePaymentRequestAsync(int paymentRequestId, PaymentRequest paymentRequest)
        {
            var existingRequest = await _context.PaymentRequests.FindAsync(paymentRequestId);
            if (existingRequest == null) return null;

            existingRequest.CardNumber = paymentRequest.CardNumber;
            existingRequest.ExpirationDate = paymentRequest.ExpirationDate;
            existingRequest.CardholderName = paymentRequest.CardholderName;
            existingRequest.CVC = paymentRequest.CVC;
            existingRequest.CartId = paymentRequest.CartId;

            await _context.SaveChangesAsync();
            return existingRequest;
        }

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
