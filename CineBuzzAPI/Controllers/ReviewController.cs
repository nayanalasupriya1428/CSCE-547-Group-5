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
        private readonly ILogger<ReviewController> _logger;
        private readonly IMovieService _movieService;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        // POST: Add a review
        [HttpPost]
        public async Task<ActionResult<Review>> AddReview(int movieId, [FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest("Review cannot be null");
            }

            // Validation logic
            if (string.IsNullOrEmpty(review.Content) || review.ReviewScore < 1 || review.ReviewScore > 5)
            {
                return BadRequest("Invalid review data.");
            }

            try
            {
                var result = await _reviewService.AddReviewAsync(movieId, review);
                return CreatedAtAction(nameof(GetReviews), new { movieId }, result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while adding the review.");
            }
        }

        // GET: Get all reviews for a movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int movieId)
        {
            if (movieId <= 0)
            {
                return BadRequest("Invalid movie ID.");
            }

            try
            {
                var reviews = await _reviewService.GetReviewsAsync(movieId);
                if (reviews == null || reviews.Value == null || !reviews.Value.Any())
                {
                    return NotFound("No reviews found for the specified movie.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reviews.");
            }
        }

        // PUT: Edit a specific review
        [HttpPut("{reviewId}")]
        public async Task<ActionResult<Review>> EditReview(int movieId, int reviewId, [FromBody] Review newReview)
        {
            // Validation logic
            if (newReview == null)
            {
                return BadRequest("Review cannot be null");
            }

            if (string.IsNullOrEmpty(newReview.Content) || newReview.ReviewScore < 1 || newReview.ReviewScore > 5)
            {
                return BadRequest("Invalid review data.");
            }

            try
            {
                // Perform the update
                var updatedReview = await _reviewService.EditReviewAsync(movieId, reviewId, newReview);

                if (updatedReview == null)
                {
                    return NotFound("Review not found.");
                }

                return Ok(updatedReview);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while editing review {reviewId} for movie {movieId}.", reviewId, movieId);
                return StatusCode(503, "Database is currently unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing review {reviewId} for movie {movieId}.", reviewId, movieId);
                return StatusCode(500, "An error occurred while editing the review.");
            }
        }

        // DELETE: Delete a specific review
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int movieId, int reviewId)
        {
            if (movieId <= 0 || reviewId <= 0)
            {
                return BadRequest("Invalid movie ID or review ID.");
            }

            try
            {
                var result = await _reviewService.DeleteReviewAsync(movieId, reviewId);
                if (result == null)
                {
                    return NotFound("Review not found");
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting review {reviewId} for movie {movieId}.", reviewId, movieId);
                return StatusCode(503, "Database is currently unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting review {reviewId} for movie {movieId}.", reviewId, movieId);
                return StatusCode(500, "An error occurred while deleting the review.");
            }
        }
    }

}
