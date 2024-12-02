using Microsoft.AspNetCore.Mvc;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("{movieId}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int movieId)
    {
        var reviews = await _reviewService.GetReviewsByMovieIdAsync(movieId);
        return Ok(reviews);
    }

    [HttpGet("details/{reviewId}")]
    public async Task<ActionResult<Review>> GetReview(int reviewId)
    {
        var review = await _reviewService.GetReviewByIdAsync(reviewId);
        if (review == null) return NotFound("Review not found.");
        return Ok(review);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] Review review)
    {
        bool success = await _reviewService.AddReviewAsync(review);
        return success ? Ok("Review added successfully.") : BadRequest("Failed to add review.");
    }

    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] Review review)
    {
        bool success = await _reviewService.UpdateReviewAsync(reviewId, review);
        return success ? Ok("Review updated successfully.") : NotFound("Review not found.");
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        bool success = await _reviewService.DeleteReviewAsync(reviewId);
        return success ? Ok("Review removed successfully.") : NotFound("Review not found.");
    }
}
