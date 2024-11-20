using CineBuzzApi.Data;
using CineBuzzApi.Models;
using CineBuzzAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineBuzzAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly CineBuzzDbContext _context;

        public ReviewService(CineBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddReviewAsync(int movieId, [FromBody] Review review)
        {
            // Check if the input is valid. If not, do not continue with the rest of the code.
            if (movieId < 0)
            {
                throw new System.ArgumentException("Invalid movie ID.");
            }

            else if (review == null || review.Content == null)
            {
                throw new System.ArgumentException("Review cannot be null.");
            }
            else if (review.ReviewScore < 1 || review.ReviewScore > 5)
            {
                throw new System.ArgumentException("Invalid review score.");
            }

            // Check if the movie exists in the database.
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
            {
                throw new System.ArgumentException("Movie not found.");
            }

            // Apply business logic
            review.ReviewDate = System.DateTime.Now;
            review.MovieId = movieId;

            // add review
            try
            {
                _context.Add(review);
                // add movie to review
                await _context.SaveChangesAsync();
                return review;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> DeleteReviewAsync(int movieId, int reviewId)
        {
            // Implementation will be added later
            throw new System.NotImplementedException();
        }

        public Task<Review?> EditReviewAsync(int movieId, int reviewId, [FromBody] Review newReview)
        {
            // Implementation will be added later
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync(int movieId)
        {
            return await _context.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
        }

        Task<IEnumerable<Movie>> IReviewService.GetReviewsAsync(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}