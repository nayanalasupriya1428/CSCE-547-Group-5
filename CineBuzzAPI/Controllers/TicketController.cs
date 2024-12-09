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
        [HttpPost("AddTicketsToMovie/{movieTimeId}")]
   public async Task<ActionResult<Ticket>> AddTicketsToMovie(int movieTimeId, [FromBody] int numberOfTickets)
{
    var addedTicket = await _ticketService.AddTicketsAsync(movieTimeId, numberOfTickets);
    if (addedTicket == null)
        return BadRequest("Could not add tickets");
    return CreatedAtAction(nameof(Get), new { id = addedTicket.TicketId }, addedTicket);
}

[HttpDelete("RemoveTicketsFromMovie/{ticketId}")]
public async Task<ActionResult> RemoveTicketsFromMovie(int ticketId, [FromBody] int numberOfTickets)
{
    var result = await _ticketService.RemoveTicketsAsync(ticketId, numberOfTickets);
    if (!result)
        return BadRequest("Could not remove tickets or insufficient quantity");
    return NoContent();
}

[HttpPut("EditTickets/{ticketId}")]
public async Task<ActionResult<Ticket>> EditTickets(int ticketId, [FromBody] EditTicketRequest editRequest)
{
    var updatedTicket = await _ticketService.UpdateTicketAsync(ticketId, editRequest.Price, editRequest.Quantity);
    if (updatedTicket == null)
        return NotFound("Ticket not found");
    return Ok(updatedTicket);
}

    }
}
