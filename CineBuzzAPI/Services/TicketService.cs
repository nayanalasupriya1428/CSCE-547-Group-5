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

        public TicketService(CineBuzzDbContext context) => _context = context;

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync() =>
            await _context.Tickets.Include(t => t.MovieTime).ToListAsync();

        public async Task<Ticket?> GetTicketByIdAsync(int ticketId) =>
            await _context.Tickets.Include(t => t.MovieTime).FirstOrDefaultAsync(t => t.TicketId == ticketId);

        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticketId);
            if (existingTicket == null) return null;

            existingTicket.Price = ticket.Price;
            existingTicket.Quantity = ticket.Quantity;
            existingTicket.Availability = ticket.Availability;
            existingTicket.SeatNumber = ticket.SeatNumber;
            existingTicket.MovieTimeId = ticket.MovieTimeId;

            await _context.SaveChangesAsync();
            return existingTicket;
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        // Gets all tickets for a specific movie by its ID.
        public async Task<List<Ticket>> GetTicketsByMovieIdAsync(int movieId)
        {
            // Retrieve tickets where the associated MovieTime has the specified MovieId
            return await _context.Tickets
                .Include(ticket => ticket.MovieTime)
                .Where(ticket => ticket.MovieTime != null && ticket.MovieTime.MovieId == movieId)
                .ToListAsync();
        }

        public Task AddTicketsToMovieAsync(int movieId, int numberOfTickets)
        {
            var movieTimes = _context.MovieTimes.Where(mt => mt.MovieId == movieId).ToList();

            foreach (var movieTime in movieTimes)
            {
                for (int i = 0; i < numberOfTickets; i++)
                {
                    var ticket = new Ticket
                    {
                        MovieTimeId = movieTime.MovieTimeId,
                        Price = 10.0,
                        Quantity = 1,
                        Availability = true,
                        SeatNumber = i + 1
                    };
                    _context.Tickets.Add(ticket);
                }
            }

            return _context.SaveChangesAsync();

        }

        public Task RemoveTicketsFromMovieAsync(int movieId, int numberOfTickets)
        {
            throw new NotImplementedException();
        }

        public Task EditTicketsAsync(int movieId, Ticket newTicket)
        {
            throw new NotImplementedException();
        }
    }
}
