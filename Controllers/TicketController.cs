using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Models;

namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly MovieContext _context;

        public TicketController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.Include(t => t.Movie).ToListAsync();
        }

        [HttpGet("ByMovie/{movieId}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsByMovie(int movieId)
        {
            var tickets = await _context.Tickets
                                        .Where(t => t.MovieId == movieId)
                                        .Include(t => t.Movie)
                                        .ToListAsync();

            if (!tickets.Any())
            {
                return NotFound($"No tickets available for movie with ID {movieId}.");
            }

            return Ok(tickets);
        }

    }
}

