// @author Scott Do (Reshlynt)
// Unit tests for CartController class in MovieReviewApi.
// @date 2024-11-5

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Controllers;
using MovieReviewApi.Models;

namespace MovieReviewApi.Tests
{
    [TestClass]
    public class MovieControllerTests
    {
        private MovieContext _context;
        private MovieController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Set up In-Memory Database for MovieContext with unique database for each test
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString())
                .Options;

            _context = new MovieContext(options);
            SeedTestData(_context);
            _controller = new MovieController(_context);
        }

        /// <summary>
        /// Seeds the in-memory database with initial data for testing purposes.
        /// </summary>
        /// <param name="context">The in-memory movie context to be seeded with test data.</param>
        private void SeedTestData(MovieContext context)
        {
            var movies = new List<Movie>
            {
                new Movie { Id = 1, MovieTitle = "Movie 1", Genre = "Action", Rating = 4 },
                new Movie { Id = 2, MovieTitle = "Movie 2", Genre = "Comedy", Rating = 3 },
                new Movie { Id = 3, MovieTitle = "Movie 3", Genre = "Drama", Rating = 5 }
            };

            context.Movie.AddRange(movies);
            context.SaveChanges();
        }

        /// <summary>
        /// Tests that retrieving all movies returns the correct list of movies.
        /// </summary>
        [TestMethod]
        public async Task GetMovies_ShouldReturnAllMovies()
        {
            // Act
            var result = await _controller.GetMovie();

            // Assert
            Assert.IsNotNull(result);

            var movies = result.Value;
            Assert.IsNotNull(movies);
            Assert.AreEqual(3, movies.Count());

            // Optionally, verify the details of the movies
            var moviesList = movies.ToList();
            Assert.AreEqual(1, moviesList[0].Id);
            Assert.AreEqual("Movie 1", moviesList[0].MovieTitle);
            Assert.AreEqual("Action", moviesList[0].Genre);
            Assert.AreEqual(4, moviesList[0].Rating);
        }

        /// <summary>
        /// Tests that a new movie can be added successfully.
        /// </summary>
        [TestMethod]
        public async Task PostMovie_ShouldAddMovieSuccessfully()
        {
            // Arrange
            var newMovie = new Movie
            {
                MovieTitle = "New Movie",
                Genre = "Thriller",
                Rating = 5
            };

            // Act
            var result = await _controller.PostMovie(newMovie);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            var createdMovie = createdResult.Value as Movie;
            Assert.IsNotNull(createdMovie);
            Assert.AreEqual(newMovie.MovieTitle, createdMovie.MovieTitle);
            Assert.AreEqual(newMovie.Genre, createdMovie.Genre);
            Assert.AreEqual(newMovie.Rating, createdMovie.Rating);

            var movieInDb = await _context.Movie.FindAsync(createdMovie.Id);
            Assert.IsNotNull(movieInDb);
            Assert.AreEqual(newMovie.MovieTitle, movieInDb.MovieTitle);
            Assert.AreEqual(newMovie.Genre, movieInDb.Genre);
            Assert.AreEqual(newMovie.Rating, movieInDb.Rating);
        }

        /// <summary>
        /// Tests that an existing movie can be deleted successfully.
        /// </summary>
        [TestMethod]
        public async Task DeleteMovie_ShouldDeleteMovieSuccessfully()
        {
            // Arrange
            int movieIdToDelete = 1;

            // Act
            var result = await _controller.DeleteMovie(movieIdToDelete);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var movieInDb = await _context.Movie.FindAsync(movieIdToDelete);
            Assert.IsNull(movieInDb);
        }
    }
}
