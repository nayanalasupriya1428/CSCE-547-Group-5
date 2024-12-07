using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> Get()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Get(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Post([FromBody] Ticket ticket)
        {
            var createdTicket = await _ticketService.AddTicketAsync(ticket);
            return CreatedAtAction(nameof(Get), new { id = createdTicket.TicketId }, createdTicket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> Put(int id, [FromBody] Ticket ticket)
        {
            var updatedTicket = await _ticketService.UpdateTicketAsync(id, ticket);
            if (updatedTicket == null)
                return NotFound();
            return Ok(updatedTicket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
