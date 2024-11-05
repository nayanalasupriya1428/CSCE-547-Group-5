using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Models;

namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly MovieContext _context;

        public PaymentController(MovieContext context)
        {
            _context = context;
        }

        [HttpPost("ProcessPayment")]
public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
{
    // Validate the cart
    var cart = await _context.Carts.Include(c => c.CartItems)
                                   .ThenInclude(ci => ci.Ticket)
                                   .FirstOrDefaultAsync(c => c.CartId == paymentRequest.CartId);

    if (cart == null || !cart.CartItems.Any())
    {
        return BadRequest("Cart is empty or does not exist.");
    }

    // Basic payment validation (for demo purposes)
    if (string.IsNullOrWhiteSpace(paymentRequest.CardNumber) || paymentRequest.CardNumber.Length != 16)
    {
        return BadRequest("Invalid card number.");
    }

    if (string.IsNullOrWhiteSpace(paymentRequest.ExpirationDate) || 
        !DateTime.TryParse(paymentRequest.ExpirationDate, out DateTime expDate) || 
        expDate < DateTime.Now)
    {
        return BadRequest("Invalid or expired card.");
    }

    if (string.IsNullOrWhiteSpace(paymentRequest.CardholderName))
    {
        return BadRequest("Cardholder name is required.");
    }

    if (string.IsNullOrWhiteSpace(paymentRequest.CVC) || paymentRequest.CVC.Length != 3)
    {
        return BadRequest("Invalid CVC.");
    }

    // Simulate a payment process
    var totalAmount = cart.CartItems.Sum(item => item.Quantity * item.Ticket.Price);
    var paymentSuccess = true; // Simulate payment success

    if (paymentSuccess)
    {
        // Clear the cart or mark it as paid
        cart.CartItems.Clear();
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Payment successful!", TotalAmount = totalAmount });
    }
    else
    {
        return StatusCode(500, "Payment processing failed. Please try again later.");
    }
}
    }
}