using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface IPaymentRequestService
    {
        Task<IEnumerable<PaymentRequest>> GetAllPaymentRequestsAsync();
        Task<PaymentRequest?> GetPaymentRequestByIdAsync(int paymentRequestId);
        Task<PaymentRequest> AddPaymentRequestAsync(PaymentRequest paymentRequest);
        Task<PaymentRequest?> UpdatePaymentRequestAsync(int paymentRequestId, PaymentRequest paymentRequest);
        Task DeletePaymentRequestAsync(int paymentRequestId);
    }
}
