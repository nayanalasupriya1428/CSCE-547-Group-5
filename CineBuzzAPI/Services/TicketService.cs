using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Service for managing cinema tickets.
    public class TicketService : ITicketService
    {
        // Database context for accessing the database.
        private readonly CineBuzzDbContext _context;

        // Constructor that sets up the service with a database context.
        public TicketService(CineBuzzDbContext context)
        {
            _context = context;  // Initialize the database context.
        }

        // Retrieves all tickets from the database including their associated movie times.
        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            // Include the related 'MovieTime' entity and retrieve all tickets as a list.
            return await _context.Tickets.Include(t => t.MovieTime).ToListAsync();
        }

        // Retrieves a single ticket by its ID including its movie time.
        public async Task<Ticket?> GetTicketByIdAsync(int ticketId)
        {
            // Find the first ticket that matches the provided ID, including its movie time details.
            return await _context.Tickets.Include(t => t.MovieTime).FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }
        // Gets all tickets for a specific movie by its ID.
        public async Task<List<Ticket>> GetTicketsByMovieIdAsync(int movieId)
        {
            // Retrieve tickets where the associated MovieTime has the specified MovieId
            return await _context.Tickets
                .Include(ticket => ticket.MovieTime) // Include the MovieTime navigation property
                .Where(ticket => ticket.MovieTime != null && ticket.MovieTime.MovieId == movieId) // Filter by MovieId
                .ToListAsync(); // Convert to a list asynchronously
        }
        // Adds a new ticket to the database and saves the change.
        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);  // Add the new ticket to the database.
            await _context.SaveChangesAsync();  // Save changes to the database.
            return ticket;  // Return the added ticket.
        }

        // Updates the details of an existing ticket.
        public async Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticketId);  // Find the existing ticket by ID.
            if (existingTicket == null) return null;  // If not found, return null.

            // Update the properties of the existing ticket with new values.
            existingTicket.Price = ticket.Price;
            existingTicket.Quantity = ticket.Quantity;
            existingTicket.Availability = ticket.Availability;
            existingTicket.SeatNumber = ticket.SeatNumber;
            existingTicket.MovieTimeId = ticket.MovieTimeId;

            await _context.SaveChangesAsync();  // Save the updated details to the database.
            return existingTicket;  // Return the updated ticket.
        }

        // Deletes a ticket from the database.
        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);  // Find the ticket by ID.
            if (ticket != null)  // If the ticket is found,
            {
                _context.Tickets.Remove(ticket);  // Remove it from the database.
                await _context.SaveChangesAsync();  // Save changes to the database.
            }
        }
    }
}
