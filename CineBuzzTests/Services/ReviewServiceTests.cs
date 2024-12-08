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

        /// <summary>
        /// Sets up an in-memory database and initializes the ReviewService for testing.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new ReviewService(_context);
        }

        /// <summary>
        /// Cleans up the in-memory database after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        /// <summary>
        /// Tests that GetReviewsByMovieIdAsync returns only reviews associated with a specific movie ID.
        /// </summary>
        [TestMethod]
        public async Task GetReviewsByMovieIdAsync_ReturnsReviews_WhenReviewsExist()
        {
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, MovieId = 1, UserId = 1, Content = "Great movie!" },
                new Review { ReviewId = 2, MovieId = 1, UserId = 2, Content = "Amazing experience!" },
                new Review { ReviewId = 3, MovieId = 2, UserId = 3, Content = "Not bad." }
            };
            _context.Reviews.AddRange(reviews);
            await _context.SaveChangesAsync();

            var result = await _service.GetReviewsByMovieIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Tests that AddReviewAsync successfully adds a valid review.
        /// </summary>
        [TestMethod]
        public async Task AddReviewAsync_AddsReviewSuccessfully_WhenReviewIsValid()
        {
            var newReview = new Review
            {
                ReviewId = 4,
                MovieId = 1,
                UserId = 4,
                Content = "Loved it!"
            };

            var result = await _service.AddReviewAsync(newReview);

            Assert.IsTrue(result);
            Assert.AreEqual(1, await _context.Reviews.CountAsync(r => r.ReviewId == 4));
        }

        /// <summary>
        /// Tests that UpdateReviewAsync updates the content of an existing review.
        /// </summary>
        [TestMethod]
        public async Task UpdateReviewAsync_UpdatesReviewSuccessfully_WhenReviewExists()
        {
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

            var result = await _service.UpdateReviewAsync(existingReview.ReviewId, updatedReview);

            Assert.IsTrue(result);
            var reviewInDb = await _context.Reviews.FindAsync(existingReview.ReviewId);
            Assert.AreEqual("It was great!", reviewInDb.Content);
        }

        /// <summary>
        /// Tests that DeleteReviewAsync removes an existing review from the database.
        /// </summary>
        [TestMethod]
        public async Task DeleteReviewAsync_DeletesReviewSuccessfully_WhenReviewExists()
        {
            var reviewToDelete = new Review
            {
                ReviewId = 1,
                MovieId = 1,
                UserId = 1,
                Content = "This movie was okay."
            };

            _context.Reviews.Add(reviewToDelete);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteReviewAsync(reviewToDelete.ReviewId);

            Assert.IsTrue(result);
            var deletedReview = await _context.Reviews.FindAsync(reviewToDelete.ReviewId);
            Assert.IsNull(deletedReview);
        }
    }
}
