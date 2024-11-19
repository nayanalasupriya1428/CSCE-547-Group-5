using CineBuzzApi.Models;
using CineBuzzAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            if (review == null)
            {
                return BadRequest("Review cannot be null");
            }

            var result = await _reviewService.AddReview(movieId, review);
            return CreatedAtAction(nameof(GetReviews), new { movieId }, result);
        }

        // GET: Get all reviews for a movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int movieId)
        {
            var reviews = await _reviewService.GetReviews(movieId);
            return Ok(reviews);
        }

        // PUT: Edit a specific review
        [HttpPut("{reviewId}")]
        public async Task<ActionResult<Review>> EditReview(int movieId, int reviewId, [FromBody] Review newReview)
        {
            var updatedReview = await _reviewService.EditReview(movieId, reviewId, newReview);
            if (updatedReview == null)
            {
                return NotFound("Review not found");
            }

            return Ok(updatedReview);
        }

        // DELETE: Delete a specific review
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int movieId, int reviewId)
        {
            var result = await _reviewService.DeleteReview(movieId, reviewId) as StatusCodeResult;
            if (result == null || result.StatusCode != 204)
            {
                return NotFound("Review not found");
            }

            return NoContent();
        }
    }

}
