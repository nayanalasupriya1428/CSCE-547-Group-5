using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    // Marks the class as an API controller with automatic HTTP 400 responses and route setup.
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        // Private field for accessing movie services.
        private readonly IMovieService _movieService;

        // Constructor to inject the movie service dependency.
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService; // Initialize movie service.
        }

        // Gets all movies using HTTP GET and returns them.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync(); // Retrieve all movies asynchronously.
            return Ok(movies); // Return list of movies with OK status (HTTP 200).
        }

        // Adds a new movie using HTTP POST. The movie data is received in the request body.
        [HttpPost("AddMovie")]
        public async Task<ActionResult<Movie>> AddMovie([FromBody] Movie movie)
        {
            var createdMovie = await _movieService.AddMovieAsync(movie); // Add the movie asynchronously and return the created movie.
            return CreatedAtAction(nameof(GetMovies), new { id = createdMovie.MovieId }, createdMovie); // Return the created movie with HTTP 201 status.
        }

        // Removes a movie using HTTP DELETE. The movieId is specified in the URL.
        [HttpDelete("RemoveMovie/{movieId}")]
        public async Task<IActionResult> RemoveMovie(int movieId)
        {
            var success = await _movieService.RemoveMovieAsync(movieId); // Attempt to remove the movie by ID.
            if (!success) return NotFound(new { Message = "Movie not found" }); // If not successful, return HTTP 404 with a message.

            return Ok(new { Message = "Movie removed successfully" }); // If successful, return HTTP 200 with a confirmation message.
        }
    }
}
