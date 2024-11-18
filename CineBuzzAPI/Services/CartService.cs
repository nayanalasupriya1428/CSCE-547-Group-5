using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace CineBuzzApi.Services
{
    public class CartService : ICartService
    {
        private readonly CineBuzzDbContext _context;

        public CartService(CineBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(int? cartId)
        {
            Cart cart = null;

            if (cartId.HasValue)
            {
                cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Ticket)
                    .FirstOrDefaultAsync(c => c.CartId == cartId);
            }

            if (cart == null)
            {
                cart = new Cart();
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> AddTicketToCartAsync(int cartId, int ticketId, int quantity)
        {
            var cart = await GetCartAsync(cartId);
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null) throw new System.Exception("Ticket not found");

            var cartItem = cart.Items.FirstOrDefault(i => i.TicketId == ticketId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    TicketId = ticketId,
                    CartId = cart.CartId,
                    Quantity = quantity,
                    Ticket = ticket
                };
                cart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> RemoveTicketFromCartAsync(int cartId, int ticketId)
        {
            var cart = await GetCartAsync(cartId);

            var cartItem = cart.Items.FirstOrDefault(i => i.TicketId == ticketId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
