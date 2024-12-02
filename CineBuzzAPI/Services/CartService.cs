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

        // Constructor injecting the database context into the service for database operations
        public CartService(CineBuzzDbContext context)
        {
            _context = context;
        }

        // Retrieves a cart by its ID or creates a new one if it doesn't exist
        public async Task<Cart> GetCartAsync(int? cartId)
        {
            Cart cart = null;

            // Check if a cart ID was provided and attempt to retrieve the cart
            if (cartId.HasValue)
            {
                cart = await _context.Carts
                    .Include(c => c.Items)             // Include related items in the cart
                    .ThenInclude(i => i.Ticket)        // Include related ticket details for each item
                    .FirstOrDefaultAsync(c => c.CartId == cartId);
            }

            // If no cart is found, create a new empty cart
            if (cart == null)
            {
                cart = new Cart();
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();  // Save the new cart to the database
            }

            return cart;
        }

        // Adds a ticket to the specified cart with a given quantity
        public async Task<Cart> AddTicketToCartAsync(int cartId, int ticketId, int quantity)
        {
            var cart = await GetCartAsync(cartId);
            var ticket = await _context.Tickets.FindAsync(ticketId);

            // Exception handling if the ticket is not found
            if (ticket == null) throw new System.Exception("Ticket not found");

            // Check if the ticket already exists in the cart
            var cartItem = cart.Items.FirstOrDefault(i => i.TicketId == ticketId);
            if (cartItem == null)
            {
                // If not, create a new cart item and add it to the cart
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
                // If it exists, just increase the quantity
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();  // Save changes to the database
            return cart;
        }

        // Removes a specific ticket from the cart
        public async Task<Cart> RemoveTicketFromCartAsync(int cartId, int ticketId)
        {
            var cart = await GetCartAsync(cartId);

            // Find the item in the cart
            var cartItem = cart.Items.FirstOrDefault(i => i.TicketId == ticketId);
            if (cartItem != null)
            {
                // Remove the item from the cart
                cart.Items.Remove(cartItem);
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();  // Apply the changes to the database
            }

            return cart;
        }
    }
}
