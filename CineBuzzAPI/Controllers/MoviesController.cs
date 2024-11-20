using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpPost("AddMovie")]
        public async Task<ActionResult<Movie>> AddMovie([FromBody] Movie movie)
        {
            var createdMovie = await _movieService.AddMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovies), new { id = createdMovie.MovieId }, createdMovie);
        }

        [HttpDelete("RemoveMovie/{movieId}")]
        public async Task<IActionResult> RemoveMovie(int movieId)
        {
            var success = await _movieService.RemoveMovieAsync(movieId);
            if (!success) return NotFound(new { Message = "Movie not found" });

            return Ok(new { Message = "Movie removed successfully" });
        }

        [HttpPut("UpdateMovie/{movieId}")]
        public async Task<ActionResult<Movie>> EditMovie(int movieId, [FromBody] Movie movie)
        {
            var updatedMovie = await _movieService.EditMovieAsync(movieId, movie);
            if (updatedMovie == null) return NotFound(new { Message = "Movie not found" });

            return Ok(updatedMovie);
        }
    }
}
