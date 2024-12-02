using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    // Decorates the class as a controller with routing and API behavior.
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        // Dependency injection to obtain ICartService for cart operations.
        private readonly ICartService _cartService;

        // Constructor that initializes the controller with a cart service.
        public CartsController(ICartService cartService)
        {
            _cartService = cartService; // Store the provided cart service in a private field.
        }

        // HTTP GET method to retrieve a cart. Optional cartId can be provided.
        [HttpGet("{cartId?}")]
        public async Task<ActionResult<Cart>> GetCart(int? cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId); // Asynchronously get the cart from the service.
            return Ok(cart); // Return the cart with an HTTP 200 status code.
        }

        // HTTP POST method to add a ticket to a cart with specified IDs and quantity.
        [HttpPost("AddTicketToCart")]
        public async Task<ActionResult<Cart>> AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            var cart = await _cartService.AddTicketToCartAsync(cartId, ticketId, quantity); // Asynchronously add a ticket to the cart.
            return Ok(cart); // Return the updated cart with an HTTP 200 status code.
        }

        // HTTP PUT method to remove a ticket from a cart using cart and ticket IDs.
        [HttpPut("RemoveTicketFromCart")]
        public async Task<ActionResult<Cart>> RemoveTicketFromCart(int cartId, int ticketId)
        {
            var cart = await _cartService.RemoveTicketFromCartAsync(cartId, ticketId); // Asynchronously remove the ticket from the cart.
            return Ok(cart); // Return the updated cart with an HTTP 200 status code.
        }
    }
}
