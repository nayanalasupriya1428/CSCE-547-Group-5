using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentRequestsController : ControllerBase
    {
        private readonly IPaymentRequestService _paymentRequestService;

        public PaymentRequestsController(IPaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }

        // ProcessPayment endpoint
        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            // Validate card number (simple check for length and numeric content)
            if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length < 13 || request.CardNumber.Length > 19 || !Regex.IsMatch(request.CardNumber, @"^\d+$"))
            {
                return BadRequest(new { Message = "Invalid card number" });
            }

            // Validate expiration date (basic format check MM/YY)
            if (string.IsNullOrWhiteSpace(request.ExpirationDate) || !Regex.IsMatch(request.ExpirationDate, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                return BadRequest(new { Message = "Invalid expiration date format" });
            }

            // Validate cardholder name (should not be empty)
            if (string.IsNullOrWhiteSpace(request.CardholderName))
            {
                return BadRequest(new { Message = "Cardholder name is required" });
            }

            // Validate CVC (3 or 4 digits)
            if (string.IsNullOrWhiteSpace(request.CVC) || !Regex.IsMatch(request.CVC, @"^\d{3,4}$"))
            {
                return BadRequest(new { Message = "Invalid CVC" });
            }

            // Assuming the payment is processed successfully if validations pass
            var paymentRequest = new PaymentRequest
            {
                CartId = request.CartId,
                CardNumber = request.CardNumber,
                ExpirationDate = request.ExpirationDate,
                CardholderName = request.CardholderName,
                CVC = request.CVC
            };

            await _paymentRequestService.AddPaymentRequestAsync(paymentRequest);

            return Ok(new { Message = "Payment processed successfully" });
        }
    }
}
