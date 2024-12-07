using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace CineBuzzTests.Controllers
{
    [TestClass]
    public class RecentlyViewedMoviesControllerTests
    {
        private Mock<IRecentlyViewedMoviesService> _mockRecentlyViewedMoviesService;
        private RecentlyViewedMoviesController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the mock service
            _mockRecentlyViewedMoviesService = new Mock<IRecentlyViewedMoviesService>();

            // Instantiate the controller with the mock service
            _controller = new RecentlyViewedMoviesController(_mockRecentlyViewedMoviesService.Object);
        }

        [TestMethod]
        public void AddMovieToRecentlyViewed_ValidMovie_ReturnsOk()
        {
            // Arrange
            var movie = new Movie
            {
                MovieId = 1,
                Title = "Test Movie",
                Description = "A test description for the movie.",
                Genres = new List<string> { "Drama", "Adventure" }
            };

            _mockRecentlyViewedMoviesService.Setup(service => service.AddMovieToRecentlyViewed(movie));

            // Act
            var result = _controller.AddMovieToRecentlyViewed(movie);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Movie added to recently viewed list.", okResult.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }
        [TestMethod]
        public void GetRecentlyViewedMovies_ReturnsListOfMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Test Movie 1", Description = "Description 1", Genres = new List<string> { "Drama" } },
                new Movie { MovieId = 2, Title = "Test Movie 2", Description = "Description 2", Genres = new List<string> { "Action" } }
            };

            _mockRecentlyViewedMoviesService.Setup(service => service.GetRecentlyViewedMovies()).Returns(movies);

            // Act
            var result = _controller.GetRecentlyViewedMovies();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(movies, okResult.Value);
        }
    }
}
