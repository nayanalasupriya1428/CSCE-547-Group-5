using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.Include(t => t.MovieTime).ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.Include(t => t.MovieTime).FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket?> UpdateTicketAsync(int ticketId, Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
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
            if (ticket == null) return;
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddTicketsToMovieAsync(int movieId, int numberOfTickets)
        {
            var movieTime = await _context.MovieTimes.FirstOrDefaultAsync(mt => mt.MovieId == movieId);
            if (movieTime == null)
            {
                return false;
            }

            var seatNumber = await CalculateNextAvailableSeatNumber(movieTime.MovieTimeId);
            var ticket = new Ticket
            {
                MovieTimeId = movieTime.MovieTimeId,
                Price = 10.0, // Default price, should be determined by business logic
                Quantity = numberOfTickets,
                Availability = true,
                SeatNumber = seatNumber
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTicketsFromMovieAsync(int movieId, int numberOfTickets)
        {
            var tickets = await _context.Tickets
                                       .Where(t => t.MovieTime.MovieId == movieId)
                                       .ToListAsync();

            if (tickets == null || tickets.Count == 0) return false;

            int ticketsToRemove = numberOfTickets;
            tickets = tickets.OrderBy(t => t.SeatNumber).ToList();

            foreach (var ticket in tickets)
            {
                if (ticketsToRemove <= 0) break;

                if (ticket.Quantity > ticketsToRemove)
                {
                    ticket.Quantity -= ticketsToRemove;
                    ticketsToRemove = 0;
                }
                else
                {
                    ticketsToRemove -= ticket.Quantity;
                    ticket.Quantity = 0;
                    ticket.Availability = false;
                }

                _context.Tickets.Update(ticket);
            }

            await _context.SaveChangesAsync();
            return ticketsToRemove == 0;
        }

        public async Task<bool> EditTicketsAsync(int ticketId, EditTicketRequest newTicketDetails)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            ticket.Price = newTicketDetails.Price ?? ticket.Price;
            ticket.Quantity = newTicketDetails.Quantity ?? ticket.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<int> CalculateNextAvailableSeatNumber(int movieTimeId)
        {
            var highestSeatNumber = await _context.Tickets
                                                  .Where(t => t.MovieTimeId == movieTimeId)
                                                  .MaxAsync(t => (int?)t.SeatNumber) ?? 0;

            return highestSeatNumber + 1;
        }
    }
}
