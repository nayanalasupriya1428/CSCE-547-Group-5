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
        Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket);
        Task DeleteTicketAsync(int ticketId);
    }
}

