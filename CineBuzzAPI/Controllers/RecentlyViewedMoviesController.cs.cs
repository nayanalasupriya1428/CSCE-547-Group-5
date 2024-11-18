using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CineBuzzApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecentlyViewedMoviesController : ControllerBase
    {
        private readonly IRecentlyViewedMoviesService _recentlyViewedMoviesService;

        public RecentlyViewedMoviesController(IRecentlyViewedMoviesService recentlyViewedMoviesService)
        {
            _recentlyViewedMoviesService = recentlyViewedMoviesService;
        }

        [HttpPost("AddMovie")]
        public IActionResult AddMovieToRecentlyViewed([FromBody] Movie movie)
        {
            _recentlyViewedMoviesService.AddMovieToRecentlyViewed(movie);
            return Ok(new { Message = "Movie added to recently viewed list." });
        }

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetRecentlyViewedMovies()
        {
            var recentlyViewedMovies = _recentlyViewedMoviesService.GetRecentlyViewedMovies();
            return Ok(recentlyViewedMovies);
        }
    }
}
