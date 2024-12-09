using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface ITicketService
    {
        // 1. Retrieves all tickets from the database asynchronously.
        // This method returns a list of all Ticket objects.
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();

        // 2. Retrieves a single ticket by its ID asynchronously.
        // It returns a specific Ticket object based on the provided ticketId.
        // If no ticket is found, it returns null.
        Task<Ticket?> GetTicketByIdAsync(int ticketId);

        // 3. Retrieves all tickets associated with a specific movie ID.
        // This method filters tickets by the movieId and returns all tickets related to that movie.
        Task<List<Ticket>> GetTicketsByMovieIdAsync(int movieId);

        // 4. Adds a new ticket to the database asynchronously.
        // This method accepts a Ticket object, saves it to the database, 
        // and then returns the added Ticket object.
        Task<Ticket> AddTicketAsync(Ticket ticket);

        // 5. Updates an existing ticket's details asynchronously.
        // This method allows updating properties of an existing ticket, such as price, quantity, etc.
        // It takes the ticketId and a Ticket object with the new values.
        // Returns the updated Ticket object or null if the ticketId doesn't exist.
        Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket);

        // 6. Deletes a specific ticket by its ID asynchronously.
        // This method removes the ticket from the database based on the provided ticketId.
        // No return value is needed.
        Task DeleteTicketAsync(int ticketId);

        // 7. Adds a specified number of tickets to a movie asynchronously.
        // This method allows adding a set number of tickets for a particular movie by the movieId.
        // Each ticket will be added to the database with default or provided values for the ticket properties.
        Task AddTicketsToMovieAsync(int movieId, int numberOfTickets);

        // 8. Removes a specified number of tickets from a movie asynchronously.
        // This method removes a given number of tickets associated with a specific movieId.
        // It ensures that there are enough tickets to remove and throws an exception if not.
        Task RemoveTicketsFromMovieAsync(int movieId, int numberOfTickets);

        // 9. Edits ticket details for a movie asynchronously.
        // This method allows updating ticket information (e.g., price, quantity, etc.) for a specific movieId.
        // The newTicket object contains updated values for the ticket properties.
        Task EditTicketsAsync(int movieId, Ticket newTicket);
    }
}

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

        // Gets a list of tickets for a specific movie by its ID asynchronously.
        Task<List<Ticket>> GetTicketsByMovieIdAsync(int movieId);
    }
}

