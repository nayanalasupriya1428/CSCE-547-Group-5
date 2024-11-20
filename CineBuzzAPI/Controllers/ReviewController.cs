using CineBuzzApi.Models;
using CineBuzzApi.Services;
using CineBuzzAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineBuzzAPI.Controllers
{
    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // POST: Add a review
        [HttpPost]
        public async Task<ActionResult<Review>> AddReview(int movieId, [FromBody] Review review)
        {
            var _review = await _reviewService.AddReviewAsync(movieId, review);
            return CreatedAtAction(nameof(GetReviews), new { movieId = movieId }, _review);

        }

        // GET: Get all reviews for a movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int movieId)
        {
            var reviews = await _reviewService.GetReviewsAsync(movieId);
            return Ok(reviews);
        }

        // PUT: Edit a specific review
        [HttpPut("{reviewId}")]
        public async Task<ActionResult<Review>> EditReview(int movieId, int reviewId, [FromBody] Review newReview)
        {
            var review = await _reviewService.EditReviewAsync(movieId, reviewId, newReview);
            return Ok(review);
        }

        // DELETE: Delete a specific review
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int movieId, int reviewId)
        {
            var success = await _reviewService.DeleteReviewAsync(movieId, reviewId);
            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }

}
