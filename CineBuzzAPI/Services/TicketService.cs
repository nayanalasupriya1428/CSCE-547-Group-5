using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly CineBuzzDbContext _context;

        public TicketService(CineBuzzDbContext context)
        {
            _context = context;
        }

        // Gets all tickets from the database.
        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.Include(t => t.MovieTime).ToListAsync();
        }

        // Gets a ticket by its ID.
        public async Task<Ticket?> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.Include(t => t.MovieTime).FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }
        public async Task<List<Ticket>> GetTicketsByMovieIdAsync(int movieId)
        {
            return await _context.Tickets
                .Include(ticket => ticket.MovieTime)
                .Where(ticket => ticket.MovieTime != null && ticket.MovieTime.MovieId == movieId)
                .ToListAsync();
        }
        // Adds a new ticket to the database and saves the change.
        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        // Updates the details of an existing ticket.
        public async Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticketId);
            if (existingTicket == null) return null;

            // Update the properties of the existing ticket with new values.
            existingTicket.Price = ticket.Price;
            existingTicket.Quantity = ticket.Quantity;
            existingTicket.Availability = ticket.Availability;
            existingTicket.SeatNumber = ticket.SeatNumber;
            existingTicket.MovieTimeId = ticket.MovieTimeId;

            await _context.SaveChangesAsync();
            return existingTicket;
        }

        // Deletes a ticket from the database.
        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
        // Adds a default ticket to a movie based on the movieId and number of tickets.
        public async Task<bool> AddTicketsToMovieAsync(int movieId, int numberOfTickets)
        {
            if (numberOfTickets <= 0)
                return false;

            var movieTimes = await _context.MovieTimes
                .Where(mt => mt.MovieId == movieId)
                .ToListAsync();

            if (movieTimes == null || !movieTimes.Any())
                return false;

            var newTickets = new List<Ticket>();
            foreach (var movieTime in movieTimes)
            {
                for (int i = 0; i < numberOfTickets; i++)
                {
                    newTickets.Add(new Ticket
                    {
                        MovieTimeId = movieTime.MovieTimeId,
                        Price = 10.0,
                        Quantity = 1,
                        Availability = true,
                        SeatNumber = i + 1
                    });
                }
            }

            _context.Tickets.AddRange(newTickets);
            await _context.SaveChangesAsync();

            return true;
        }
        // Removes tickets from a movie based on the movieId and number of tickets.
        public async Task<bool> RemoveTicketsFromMovieAsync(int movieId, int numberOfTickets)
        {
            if (numberOfTickets <= 0)
                return false;

            var movieTimes = await _context.MovieTimes
                .Where(mt => mt.MovieId == movieId)
                .Include(mt => mt.Tickets)
                .ToListAsync();

            if (movieTimes == null || !movieTimes.Any())
                return false;

            int ticketsToRemove = numberOfTickets;
            var ticketsToDelete = new List<Ticket>();

            foreach (var movieTime in movieTimes)
            {
                var availableTickets = movieTime.Tickets.Take(ticketsToRemove).ToList();
                ticketsToDelete.AddRange(availableTickets);

                ticketsToRemove -= availableTickets.Count;

                if (ticketsToRemove <= 0)
                    break;
            }

            if (ticketsToRemove > 0)
                return false;

            // Remove the tickets
            _context.Tickets.RemoveRange(ticketsToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}