using CineBuzzApi.Models;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Interface defining the contract for cart-related operations.
    public interface ICartService
    {
        // Retrieves a cart by its ID asynchronously. If no cart ID is provided, it may return a default or new cart.
        Task<Cart> GetCartAsync(int? cartId);

        // Adds a ticket to a specified cart asynchronously, given the cart ID, ticket ID, and the quantity of tickets to add.
        Task<Cart> AddTicketToCartAsync(int cartId, int ticketId, int quantity);

        // Removes a ticket from a specified cart asynchronously, given the cart ID and ticket ID.
        Task<Cart> RemoveTicketFromCartAsync(int cartId, int ticketId);
    }
}