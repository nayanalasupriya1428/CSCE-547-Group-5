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
            // Initialize the mock service
            _mockReviewService = new Mock<IReviewService>();

            // Instantiate the controller with the mock service
            _controller = new ReviewsController(_mockReviewService.Object);
        }

        [TestMethod]
        public async Task GetReviewsByMovieId_ValidMovieId_ReturnsReviews()
        {
            // Arrange
            int movieId = 1;
            var reviews = new List<Review>
    {
        new Review { ReviewId = 1, UserId = 101, MovieId = movieId, Content = "Great movie!" },
        new Review { ReviewId = 2, UserId = 102, MovieId = movieId, Content = "Not bad, enjoyed it." }
    };

            _mockReviewService
                .Setup(service => service.GetReviewsByMovieIdAsync(movieId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetReviewsByMovieId(movieId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(reviews, okResult.Value);
        }
        [TestMethod]
        public async Task GetReview_ValidReviewId_ReturnsReview()
        {
            // Arrange
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

            // Act
            var result = await _controller.GetReview(reviewId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(review, okResult.Value);
        }
        [TestMethod]
        public async Task GetReview_InvalidReviewId_ReturnsNotFound()
        {
            // Arrange
            int reviewId = 999; // An ID that does not exist
            _mockReviewService
                .Setup(service => service.GetReviewByIdAsync(reviewId))
                .ReturnsAsync((Review)null); // Simulate no review found

            // Act
            var result = await _controller.GetReview(reviewId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Review not found.", notFoundResult.Value);
        }
        [TestMethod]
        public async Task AddReview_ValidReview_ReturnsOk()
        {
            // Arrange
            var review = new Review
            {
                ReviewId = 1,
                UserId = 101,
                MovieId = 1,
                Content = "Amazing storyline and visuals!"
            };

            _mockReviewService
                .Setup(service => service.AddReviewAsync(review))
                .ReturnsAsync(true); // Simulate successful addition

            // Act
            var result = await _controller.AddReview(review);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Review added successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task AddReview_InvalidReview_ReturnsBadRequest()
        {
            // Arrange
            var review = new Review
            {
                ReviewId = 1,
                UserId = 101,
                MovieId = 1,
                Content = "Amazing storyline and visuals!"
            };

            _mockReviewService
                .Setup(service => service.AddReviewAsync(review))
                .ReturnsAsync(false); // Simulate failure

            // Act
            var result = await _controller.AddReview(review);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Failed to add review.", badRequestResult.Value);
        }
        [TestMethod]
        public async Task UpdateReview_ValidReviewIdAndData_ReturnsOk()
        {
            // Arrange
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
                .ReturnsAsync(true); // Simulate successful update

            // Act
            var result = await _controller.UpdateReview(reviewId, updatedReview);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Review updated successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task UpdateReview_InvalidReviewId_ReturnsNotFound()
        {
            // Arrange
            int reviewId = 999; // Nonexistent review ID
            var updatedReview = new Review
            {
                ReviewId = reviewId,
                UserId = 101,
                MovieId = 1,
                Content = "Updated review content"
            };

            _mockReviewService
                .Setup(service => service.UpdateReviewAsync(reviewId, updatedReview))
                .ReturnsAsync(false); // Simulate failure due to invalid ID

            // Act
            var result = await _controller.UpdateReview(reviewId, updatedReview);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Review not found.", notFoundResult.Value);
        }
        [TestMethod]
        public async Task DeleteReview_ValidReviewId_ReturnsOk()
        {
            // Arrange
            int reviewId = 1;

            _mockReviewService
                .Setup(service => service.DeleteReviewAsync(reviewId))
                .ReturnsAsync(true); // Simulate successful deletion

            // Act
            var result = await _controller.DeleteReview(reviewId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Review removed successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteReview_InvalidReviewId_ReturnsNotFound()
        {
            // Arrange
            int reviewId = 999; // Nonexistent review ID

            _mockReviewService
                .Setup(service => service.DeleteReviewAsync(reviewId))
                .ReturnsAsync(false); // Simulate failure due to invalid ID

            // Act
            var result = await _controller.DeleteReview(reviewId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Review not found.", notFoundResult.Value);
        }

    }
}
