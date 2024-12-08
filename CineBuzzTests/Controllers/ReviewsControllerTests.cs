using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CineBuzzTests.Controllers
{
    [TestClass]
    public class ReviewsControllerTests
    {
        private Mock<IReviewService> _mockReviewService;
        private ReviewsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockReviewService = new Mock<IReviewService>();
            _controller = new ReviewsController(_mockReviewService.Object);
        }

        /// <summary>
        /// Tests if GetReviewsByMovieId returns reviews for a valid movie ID.
        /// </summary>
        [TestMethod]
        public async Task GetReviewsByMovieId_ValidMovieId_ReturnsReviews()
        {
            int movieId = 1;
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, UserId = 101, MovieId = movieId, Content = "Great movie!" },
                new Review { ReviewId = 2, UserId = 102, MovieId = movieId, Content = "Not bad, enjoyed it." }
            };

            _mockReviewService
                .Setup(service => service.GetReviewsByMovieIdAsync(movieId))
                .ReturnsAsync(reviews);

            var result = await _controller.GetReviewsByMovieId(movieId);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(reviews, okResult.Value);
        }

        /// <summary>
        /// Tests if GetReview returns the correct review for a valid review ID.
        /// </summary>
        [TestMethod]
        public async Task GetReview_ValidReviewId_ReturnsReview()
        {
            int reviewId = 1;
            var review = new Review
            {
                ReviewId = reviewId,
                UserId = 101,
                MovieId = 1,
                Content = "Excellent movie!"
            };

            _mockReviewService
                .Setup(service => service.GetReviewByIdAsync(reviewId))
                .ReturnsAsync(review);

            var result = await _controller.GetReview(reviewId);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(review, okResult.Value);
        }

        /// <summary>
        /// Tests if GetReview returns NotFound for an invalid review ID.
        /// </summary>
        [TestMethod]
        public async Task GetReview_InvalidReviewId_ReturnsNotFound()
        {
            int reviewId = 999;
            _mockReviewService
                .Setup(service => service.GetReviewByIdAsync(reviewId))
                .ReturnsAsync((Review)null);

            var result = await _controller.GetReview(reviewId);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Review not found.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests if AddReview adds a valid review and returns Ok.
        /// </summary>
        [TestMethod]
        public async Task AddReview_ValidReview_ReturnsOk()
        {
            var review = new Review
            {
                ReviewId = 1,
                UserId = 101,
                MovieId = 1,
                Content = "Amazing storyline and visuals!"
            };

            _mockReviewService
                .Setup(service => service.AddReviewAsync(review))
                .ReturnsAsync(true);

            var result = await _controller.AddReview(review);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Review added successfully.", okResult.Value);
        }

        /// <summary>
        /// Tests if AddReview returns BadRequest for an invalid review.
        /// </summary>
        [TestMethod]
        public async Task AddReview_InvalidReview_ReturnsBadRequest()
        {
            var review = new Review
            {
                ReviewId = 1,
                UserId = 101,
                MovieId = 1,
                Content = "Amazing storyline and visuals!"
            };

            _mockReviewService
                .Setup(service => service.AddReviewAsync(review))
                .ReturnsAsync(false);

            var result = await _controller.AddReview(review);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Failed to add review.", badRequestResult.Value);
        }

        /// <summary>
        /// Tests if UpdateReview updates a valid review and returns Ok.
        /// </summary>
        [TestMethod]
        public async Task UpdateReview_ValidReviewIdAndData_ReturnsOk()
        {
            int reviewId = 1;
            var updatedReview = new Review
            {
                ReviewId = reviewId,
                UserId = 101,
                MovieId = 1,
                Content = "Updated review content"
            };

            _mockReviewService
                .Setup(service => service.UpdateReviewAsync(reviewId, updatedReview))
                .ReturnsAsync(true);

            var result = await _controller.UpdateReview(reviewId, updatedReview);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Review updated successfully.", okResult.Value);
        }

        /// <summary>
        /// Tests if UpdateReview returns NotFound for an invalid review ID.
        /// </summary>
        [TestMethod]
        public async Task UpdateReview_InvalidReviewId_ReturnsNotFound()
        {
            int reviewId = 999;
            var updatedReview = new Review
            {
                ReviewId = reviewId,
                UserId = 101,
                MovieId = 1,
                Content = "Updated review content"
            };

            _mockReviewService
                .Setup(service => service.UpdateReviewAsync(reviewId, updatedReview))
                .ReturnsAsync(false);

            var result = await _controller.UpdateReview(reviewId, updatedReview);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Review not found.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests if DeleteReview removes a valid review and returns Ok.
        /// </summary>
        [TestMethod]
        public async Task DeleteReview_ValidReviewId_ReturnsOk()
        {
            int reviewId = 1;

            _mockReviewService
                .Setup(service => service.DeleteReviewAsync(reviewId))
                .ReturnsAsync(true);

            var result = await _controller.DeleteReview(reviewId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Review removed successfully.", okResult.Value);
        }

        /// <summary>
        /// Tests if DeleteReview returns NotFound for an invalid review ID.
        /// </summary>
        [TestMethod]
        public async Task DeleteReview_InvalidReviewId_ReturnsNotFound()
        {
            int reviewId = 999;

            _mockReviewService
                .Setup(service => service.DeleteReviewAsync(reviewId))
                .ReturnsAsync(false);

            var result = await _controller.DeleteReview(reviewId);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Review not found.", notFoundResult.Value);
        }
    }
}
