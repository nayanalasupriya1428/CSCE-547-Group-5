using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Models;

namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _context;

        public MovieController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/MovieReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            return await _context.Movie.ToListAsync();
        }

        // POST: api/MovieReviews
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }
        // DELETE: api/MovieReviews/{id}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteMovie(int id)
{
    var movie = await _context.Movie.FindAsync(id);
    if (movie == null)
    {
        return NotFound();
    }

    _context.Movie.Remove(movie);
    await _context.SaveChangesAsync();

    return NoContent(); // Correct response type after a deletion
}

    }
}
