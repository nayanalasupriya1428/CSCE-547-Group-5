using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int ticketId);
        Task<Ticket> AddTicketAsync(Ticket ticket);
        Task<Ticket> AddTicketsAsync(int movieTimeId, int quantity); // New method to add tickets
        Task<bool> RemoveTicketsAsync(int ticketId, int quantity);  // New method to remove tickets
        Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket); // Existing method
        Task<Ticket> UpdateTicketAsync(int ticketId, double? price, int? quantity); // Updated method to allow flexible updates
        Task DeleteTicketAsync(int ticketId);
    }
}

