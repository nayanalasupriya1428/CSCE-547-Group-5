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

            // Validation logic
            if (string.IsNullOrEmpty(review.Content) || review.ReviewScore < 1 || review.ReviewScore > 5)
            {
                return BadRequest("Invalid review data.");
            }

            try
            {
                var result = await _reviewService.AddReview(movieId, review);
                return CreatedAtAction(nameof(GetReviews), new { movieId }, result);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                _logger.LogError(ex, "An error occurred while adding the review.");
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
                var reviews = await _reviewService.GetReviews(movieId);
                if (reviews == null || !reviews.Any())
                {
                    return NotFound("No reviews found for the specified movie.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the reviews.");
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
                var updatedReview = await _reviewService.EditReview(movieId, reviewId, newReview);

                if (updatedReview == null)
                {
                    return NotFound("Review not found");
                }

                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the review.");
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
                var result = await _reviewService.DeleteReview(movieId, reviewId);
                if (result == null)
                {
                    return NotFound("Review not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the review.");
                return StatusCode(500, "An error occurred while deleting the review.");
            }
        }
    }

}
