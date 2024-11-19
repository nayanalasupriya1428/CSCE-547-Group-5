using CineBuzzApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CineBuzzAPI.Interfaces
{
    public interface IReviewService
    {
        // Post action, add a review
        Task<ActionResult<Review>> AddReview(int movieId, [FromBody] Review review);

        // Delete action, delete a review
        Task<IActionResult> DeleteReview(int movieId, int reviewId);

        // put action, edit a review from the ID and edit accordingly
        Task<ActionResult<Review>> EditReview(int movieId, int reviewId, [FromBody] Review newReview);

        // get action, return all reviews for a movie
        Task<ActionResult<IEnumerable<Review>>> GetReviews(int movieId);
    }
}
