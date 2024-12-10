using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Interface for managing ticket-related operations.
    public interface ITicketService
    {
        // Retrieves all tickets from the database asynchronously.
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();

        // Retrieves a specific ticket by its ID asynchronously. Returns null if the ticket is not found.
        Task<Ticket?> GetTicketByIdAsync(int ticketId);

        // Adds a new ticket to the database and returns the added ticket asynchronously.
        Task<Ticket> AddTicketAsync(Ticket ticket);

        // Updates an existing ticket in the database and returns the updated ticket asynchronously.
        // Returns null if the original ticket cannot be found.
        Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket);

        // Deletes a ticket from the database based on its ID asynchronously.
        Task DeleteTicketAsync(int ticketId);
        Task<bool> AddTicketsToMovieAsync(int movieId, int numberOfTickets);
Task<bool> RemoveTicketsFromMovieAsync(int movieId, int numberOfTickets);
Task<bool> EditTicketsAsync(int ticketId, EditTicketRequest newTicketDetails);
    }
}

