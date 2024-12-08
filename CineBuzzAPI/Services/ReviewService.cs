using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{

    public class ReviewService : IReviewService
    {
        private readonly CineBuzzDbContext _context;

        public ReviewService(CineBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId)
        {
            return await _context.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateReviewAsync(int reviewId, Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(reviewId);
            if (existingReview == null) return false;
            existingReview.Content = review.Content;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return false;
            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}