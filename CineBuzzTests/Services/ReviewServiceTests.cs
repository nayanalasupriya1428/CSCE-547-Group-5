using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CineBuzzApi.Services
{
    [TestClass]
    public class ReviewServiceTests
    {
        private ReviewService _service;
        private CineBuzzDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new ReviewService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetReviewsByMovieIdAsync_ReturnsReviews_WhenReviewsExist()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, MovieId = 1, UserId = 1, Content = "Great movie!" },
                new Review { ReviewId = 2, MovieId = 1, UserId = 2, Content = "Amazing experience!" },
                new Review { ReviewId = 3, MovieId = 2, UserId = 3, Content = "Not bad." }
            };
            _context.Reviews.AddRange(reviews);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetReviewsByMovieIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Should return 2 reviews for MovieId 1
        }

        [TestMethod]
        public async Task AddReviewAsync_AddsReviewSuccessfully_WhenReviewIsValid()
        {
            // Arrange
            var newReview = new Review
            {
                ReviewId = 4,
                MovieId = 1,
                UserId = 4,
                Content = "Loved it!"
            };

            // Act
            var result = await _service.AddReviewAsync(newReview);

            // Assert
            Assert.IsTrue(result); // Verify the addition was successful
            Assert.AreEqual(1, await _context.Reviews.CountAsync(r => r.ReviewId == 4)); // Ensure the new review is in the database
        }

        [TestMethod]
        public async Task UpdateReviewAsync_UpdatesReviewSuccessfully_WhenReviewExists()
        {
            // Arrange
            var existingReview = new Review
            {
                ReviewId = 1,
                MovieId = 1,
                UserId = 1,
                Content = "It was good."
            };

            _context.Reviews.Add(existingReview);
            await _context.SaveChangesAsync();

            var updatedReview = new Review
            {
                Content = "It was great!"
            };

            // Act
            var result = await _service.UpdateReviewAsync(existingReview.ReviewId, updatedReview);

            // Assert
            Assert.IsTrue(result); // Verify the update was successful
            var reviewInDb = await _context.Reviews.FindAsync(existingReview.ReviewId);
            Assert.AreEqual("It was great!", reviewInDb.Content); // Verify that the content was updated
        }

        [TestMethod]
        public async Task DeleteReviewAsync_DeletesReviewSuccessfully_WhenReviewExists()
        {
            // Arrange
            var reviewToDelete = new Review
            {
                ReviewId = 1,
                MovieId = 1,
                UserId = 1,
                Content = "This movie was okay."
            };

            _context.Reviews.Add(reviewToDelete);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteReviewAsync(reviewToDelete.ReviewId);

            // Assert
            Assert.IsTrue(result); // Verify the deletion was successful
            var deletedReview = await _context.Reviews.FindAsync(reviewToDelete.ReviewId);
            Assert.IsNull(deletedReview); // Verify that the review no longer exists in the database
        }
    }
}
