using CineBuzzAPI.Interfaces;
using CineBuzzApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzAPI.Services
{
    public class ReviewService : IReviewService
    {
        public Task<ActionResult<Review>> AddReviewAsync(int movieId, [FromBody] Review review)
        {
            // Implementation will be added later
            throw new System.NotImplementedException();
        }

        public Task<IActionResult> DeleteReviewAsync(int movieId, int reviewId)
        {
            // Implementation will be added later
            throw new System.NotImplementedException();
        }

        public Task<ActionResult<Review>> EditReviewAsync(int movieId, int reviewId, [FromBody] Review newReview)
        {
            // Implementation will be added later
            throw new System.NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<Review>>> GetReviewsAsync(int movieId)
        {
            // Implementation will be added later
            throw new System.NotImplementedException();
        }
    }
}