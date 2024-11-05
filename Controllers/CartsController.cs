using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Models;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly MovieContext _context;

    public CartController(MovieContext context)
    {
        _context = context;
    }

    [HttpPost("AddTicketToCart")]
    public async Task<IActionResult> AddTicketToCart(int cartId, int ticketId, int quantity)
    {
        var cart = await _context.Carts.Include(c => c.CartItems)
                                       .FirstOrDefaultAsync(c => c.CartId == cartId);

        if (cart == null) return NotFound("Cart not found.");

        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null) return NotFound("Ticket not found.");

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
        if (cartItem != null) cartItem.Quantity += quantity;
        else cart.CartItems.Add(new CartItem { TicketId = ticketId, Quantity = quantity });

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Ticket added to cart successfully." });
    }

 [HttpPut("RemoveTicketFromCart")]
public async Task<IActionResult> RemoveTicketFromCart(int cartId, int ticketId)
{
    var cart = await _context.Carts.Include(c => c.CartItems)
                                   .ThenInclude(ci => ci.Ticket)
                                   .FirstOrDefaultAsync(c => c.CartId == cartId);

    if (cart == null)
    {
        return NotFound("Cart not found.");
    }

    var cartItem = cart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
    if (cartItem == null)
    {
        return NotFound("Ticket not found in cart.");
    }

    cart.CartItems.Remove(cartItem);
    await _context.SaveChangesAsync();

    var total = cart.CartItems.Sum(item => item.Quantity * item.Ticket.Price);

    return Ok(new { Cart = cart, Total = total });
}

    [HttpGet("GetCart")]
    public async Task<ActionResult<Cart>> GetCart(int? cartId = null)
    {
        Cart cart;

        if (cartId.HasValue)
        {
            cart = await _context.Carts.Include(c => c.CartItems)
                                       .ThenInclude(i => i.Ticket)
                                       .FirstOrDefaultAsync(c => c.CartId == cartId.Value);
            if (cart == null) return NotFound("Cart not found.");
        }
        else
        {
            cart = new Cart();
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return Ok(cart);
    }
}
