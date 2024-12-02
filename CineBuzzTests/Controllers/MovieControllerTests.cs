using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineBuzzTests.Controllers
{
    [TestClass()]
    public class MovieControllerTests
    {
        // Mocked CartService to isolate the controller for testing
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly MoviesController _controller;

        // Constructor to set up the test class with necessary components
        public MovieControllerTests()
        {
            // Instantiate the mock service
            _mockMovieService = new Mock<IMovieService>();

            // Pass the mocked service to the CartsController
            _controller = new MoviesController(_mockMovieService.Object);
        }

        [TestMethod]
        public async Task AddMovie_ReturnsCreatedAtAction_WithCreatedMovie()
        {
            // Arrange
            var newMovie = new Movie { MovieId = 1, Title = "Inception", Description = "A mind-bending thriller", Genres = new List<string> { "Sci-Fi", "Thriller" } };
            _mockMovieService.Setup(service => service.AddMovieAsync(newMovie)).ReturnsAsync(newMovie);

            // Act
            var result = await _controller.AddMovie(newMovie);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(Movie));
            var createdMovie = createdAtActionResult.Value as Movie;
            Assert.AreEqual(newMovie.MovieId, createdMovie.MovieId);
            Assert.AreEqual(newMovie.Title, createdMovie.Title);
            Assert.AreEqual(newMovie.Description, createdMovie.Description);
            CollectionAssert.AreEqual(newMovie.Genres, createdMovie.Genres);
        }

        [TestMethod]
        public async Task RemoveMovie_ReturnsOkResult_WhenMovieIsRemoved()
        {
            // Arrange
            var movieId = 1;
            _mockMovieService.Setup(service => service.RemoveMovieAsync(movieId)).ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveMovie(movieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Movie removed successfully", okResult.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }

        [TestMethod]
        public async Task RemoveMovie_ReturnsNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var movieId = 1;
            _mockMovieService.Setup(service => service.RemoveMovieAsync(movieId)).ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveMovie(movieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Movie not found", notFoundResult.Value.GetType().GetProperty("Message")?.GetValue(notFoundResult.Value));
        }

        [TestMethod]
        public async Task EditMovie_ReturnsOkResult_WhenMovieIsUpdated()
        {
            // Arrange
            var movieId = 1;
            var updatedMovie = new Movie { MovieId = 1, Title = "Inception", Description = "A mind-bending thriller", Genres = new List<string> { "Sci-Fi", "Thriller" } };
            _mockMovieService.Setup(service => service.EditMovieAsync(movieId, updatedMovie)).ReturnsAsync(true);

            // Act
            var result = await _controller.EditMovie(movieId, updatedMovie);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Movie updated successfully", okResult.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }
    }
}