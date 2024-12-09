using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    // Marks the class as a controller with automatic response types and routing for API.
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        // Dependency injected ticket service for handling ticket-related operations.
        private readonly ITicketService _ticketService;

        // Constructor initializes the controller with a ticket service.
        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Retrieves all tickets and returns them.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> Get()
        {
            var tickets = await _ticketService.GetAllTicketsAsync(); // Get all tickets using the service.
            return Ok(tickets); // Return the tickets with HTTP 200 status.
        }

        // Retrieves a single ticket by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Get(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id); // Fetch a ticket by ID.
            if (ticket == null)
                return NotFound(); // If not found, return HTTP 404.
            return Ok(ticket); // If found, return the ticket with HTTP 200 status.
        }

        // Gets all tickets by a movie Id.
        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<Ticket[]>> GetTicketsByMovieId(int movieId)
        {
            var tickets = await _ticketService.GetTicketsByMovieIdAsync(movieId);
            return Ok(tickets?.ToArray() ?? Array.Empty<Ticket>());
        }

        // Creates a new ticket.
        [HttpPost]
        public async Task<ActionResult<Ticket>> Post([FromBody] Ticket ticket)
        {
            var createdTicket = await _ticketService.AddTicketAsync(ticket); // Add a new ticket.
            return CreatedAtAction(nameof(Get), new { id = createdTicket.TicketId }, createdTicket); // Return the created ticket with HTTP 201 status.
        }

        // Updates an existing ticket.
        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> Put(int id, [FromBody] Ticket ticket)
        {
            var updatedTicket = await _ticketService.UpdateTicketAsync(id, ticket); // Update the ticket.
            if (updatedTicket == null)
                return NotFound(); // If the original ticket is not found, return HTTP 404.
            return Ok(updatedTicket); // If updated successfully, return the updated ticket.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(id); // Attempt to delete the ticket
                return NoContent(); // Return HTTP 204 if successful
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Return HTTP 404 if the ticket ID is not found
            }
        }
        // Return sucess/failure on adding tickets to a movie
        [HttpPost]
        public async Task<ActionResult> AddTicketsToMovie(int movieId, int numberOfTickets)
        {
            try
            {
                await _ticketService.AddTicketsToMovieAsync(movieId, numberOfTickets);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}