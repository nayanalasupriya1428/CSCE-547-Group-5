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
            _mockRecentlyViewedMoviesService = new Mock<IRecentlyViewedMoviesService>();
            _controller = new RecentlyViewedMoviesController(_mockRecentlyViewedMoviesService.Object);
        }

        /// <summary>
        /// Tests if AddMovieToRecentlyViewed adds a valid movie and returns Ok.
        /// </summary>
        [TestMethod]
        public void AddMovieToRecentlyViewed_ValidMovie_ReturnsOk()
        {
            var movie = new Movie
            {
                MovieId = 1,
                Title = "Test Movie",
                Description = "A test description for the movie.",
                Genres = new List<string> { "Drama", "Adventure" }
            };

            _mockRecentlyViewedMoviesService.Setup(service => service.AddMovieToRecentlyViewed(movie));

            var result = _controller.AddMovieToRecentlyViewed(movie);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Movie added to recently viewed list.", okResult.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }

        /// <summary>
        /// Tests if GetRecentlyViewedMovies returns a list of movies.
        /// </summary>
        [TestMethod]
        public void GetRecentlyViewedMovies_ReturnsListOfMovies()
        {
            var movies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Test Movie 1", Description = "Description 1", Genres = new List<string> { "Drama" } },
                new Movie { MovieId = 2, Title = "Test Movie 2", Description = "Description 2", Genres = new List<string> { "Action" } }
            };

            _mockRecentlyViewedMoviesService.Setup(service => service.GetRecentlyViewedMovies()).Returns(movies);

            var result = _controller.GetRecentlyViewedMovies();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(movies, okResult.Value);
        }
    }
}
