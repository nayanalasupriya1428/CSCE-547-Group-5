using CineBuzzApi.Models;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(int? cartId);
        Task<Cart> AddTicketToCartAsync(int cartId, int ticketId, int quantity);
        Task<Cart> RemoveTicketFromCartAsync(int cartId, int ticketId);
    }
}
