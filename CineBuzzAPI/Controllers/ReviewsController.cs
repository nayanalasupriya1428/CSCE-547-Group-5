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
    public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByMovieId(int movieId)
    {
        var reviews = await _reviewService.GetReviewsByMovieIdAsync(movieId);
        if (reviews == null || !reviews.Any())
            return NotFound("No reviews found for the specified movie.");
        
        return Ok(reviews);
    }

    [HttpGet("details/{reviewId}")]
    public async Task<ActionResult<Review>> GetReviewById(int reviewId)
    {
        var review = await _reviewService.GetReviewByIdAsync(reviewId);
        if (review == null)
            return NotFound("Review not found.");
        
        return Ok(review);
    }

    [HttpPost]
    public async Task<IActionResult> PostReview([FromBody] Review review)
    {
        if (await _reviewService.AddReviewAsync(review))
            return Ok("Review added successfully.");
        else
            return BadRequest("Failed to add review.");
    }

    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] Review review)
    {
        if (await _reviewService.UpdateReviewAsync(reviewId, review))
            return Ok("Review updated successfully.");
        else
            return NotFound("Review not found.");
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        if (await _reviewService.DeleteReviewAsync(reviewId))
            return Ok("Review deleted successfully.");
        else
            return NotFound("Review not found.");
    }
}
