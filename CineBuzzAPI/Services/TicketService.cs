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
        public async Task<Ticket> AddTicketsAsync(int movieTimeId, int quantity)
{
    var ticket = new Ticket
    {
        MovieTimeId = movieTimeId,
        Quantity = quantity,
        Price = 10.00,  // Default price, can be adjusted as needed
        Availability = true,  // Assuming new tickets are available by default
    };

    _context.Tickets.Add(ticket);
    await _context.SaveChangesAsync();
    return ticket;
}

public async Task<bool> RemoveTicketsAsync(int ticketId, int quantity)
{
    var ticket = await _context.Tickets.FindAsync(ticketId);
    if (ticket == null || ticket.Quantity < quantity) return false;

    ticket.Quantity -= quantity;
    if (ticket.Quantity == 0)
        _context.Tickets.Remove(ticket);
    else
        _context.Tickets.Update(ticket);

    await _context.SaveChangesAsync();
    return true;
}

public async Task<Ticket> UpdateTicketAsync(int ticketId, double? price, int? quantity)
{
    var ticket = await _context.Tickets.FindAsync(ticketId);
    if (ticket == null) return null;

    if (price.HasValue) ticket.Price = price.Value;
    if (quantity.HasValue) ticket.Quantity = quantity.Value;

    _context.Tickets.Update(ticket);
    await _context.SaveChangesAsync();
    return ticket;
}

    }
}
