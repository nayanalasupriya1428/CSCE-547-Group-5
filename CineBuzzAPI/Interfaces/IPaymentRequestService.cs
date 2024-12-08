using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Interface for managing payment request operations.
    public interface IPaymentRequestService
    {
        // Retrieves all payment requests from the database asynchronously.
        Task<IEnumerable<PaymentRequest>> GetAllPaymentRequestsAsync();

        // Retrieves a specific payment request by its ID asynchronously, returning null if not found.
        Task<PaymentRequest?> GetPaymentRequestByIdAsync(int paymentRequestId);

        // Adds a new payment request to the database and returns the newly added payment request asynchronously.
        Task<PaymentRequest> AddPaymentRequestAsync(PaymentRequest paymentRequest);

        // Updates an existing payment request with new information and returns the updated payment request asynchronously.
        // Returns null if the payment request with the specified ID does not exist.
        Task<PaymentRequest?> UpdatePaymentRequestAsync(int paymentRequestId, PaymentRequest paymentRequest);

        // Deletes a payment request from the database based on its ID asynchronously.
        Task DeletePaymentRequestAsync(int paymentRequestId);
    }
}
