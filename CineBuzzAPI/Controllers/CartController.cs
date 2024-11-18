using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{cartId?}")]
        public async Task<ActionResult<Cart>> GetCart(int? cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            return Ok(cart);
        }

        [HttpPost("AddTicketToCart")]
        public async Task<ActionResult<Cart>> AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            var cart = await _cartService.AddTicketToCartAsync(cartId, ticketId, quantity);
            return Ok(cart);
        }

        [HttpPut("RemoveTicketFromCart")]
        public async Task<ActionResult<Cart>> RemoveTicketFromCart(int cartId, int ticketId)
        {
            var cart = await _cartService.RemoveTicketFromCartAsync(cartId, ticketId);
            return Ok(cart);
        }
    }
}
