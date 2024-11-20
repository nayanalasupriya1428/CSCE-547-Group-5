using CineBuzzApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CineBuzzAPI.Interfaces
{
    public interface IReviewService
    {
        // Post action, add a review
        Task<Review> AddReviewAsync(int movieId, [FromBody] Review review);

        // Delete action, delete a review
        Task<bool> DeleteReviewAsync(int movieId, int reviewId);

        // put action, edit a review from the ID and edit accordingly
        Task<Review?> EditReviewAsync(int movieId, int reviewId, [FromBody] Review newReview);

        // get action, return all reviews for a movie
        Task<IEnumerable<Movie>> GetReviewsAsync(int movieId);
    }
}
