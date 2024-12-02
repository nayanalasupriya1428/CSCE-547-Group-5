using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<bool> AddReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(int reviewId, Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}