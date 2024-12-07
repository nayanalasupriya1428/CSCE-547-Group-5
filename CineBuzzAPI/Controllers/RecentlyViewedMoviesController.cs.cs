using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CineBuzzApi.Controllers
{
    // Defines the class as a controller with routes and API behavior.
    [ApiController]
    [Route("api/[controller]")]
    public class RecentlyViewedMoviesController : ControllerBase
    {
        // Service for managing recently viewed movies.
        private readonly IRecentlyViewedMoviesService _recentlyViewedMoviesService;

        // Constructor to inject the recently viewed movies service.
        public RecentlyViewedMoviesController(IRecentlyViewedMoviesService recentlyViewedMoviesService)
        {
            _recentlyViewedMoviesService = recentlyViewedMoviesService; // Set the service through dependency injection.
        }

        // Adds a movie to the list of recently viewed movies.
        [HttpPost("AddMovie")]
        public IActionResult AddMovieToRecentlyViewed([FromBody] Movie movie)
        {
            _recentlyViewedMoviesService.AddMovieToRecentlyViewed(movie); // Add the movie to the service.
            return Ok(new { Message = "Movie added to recently viewed list." }); // Return success message.
        }

        // Retrieves all recently viewed movies.
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetRecentlyViewedMovies()
        {
            var recentlyViewedMovies = _recentlyViewedMoviesService.GetRecentlyViewedMovies(); // Get the list from the service.
            return Ok(recentlyViewedMovies); // Return the list of movies.
        }

    }
}
