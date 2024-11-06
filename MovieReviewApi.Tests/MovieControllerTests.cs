using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            // 1. Set up In-Memory Database for MovieContext
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString()) // Use a unique database for each test
                .Options;

            _context = new MovieContext(options);

            // 2. Seed the Database with Initial Data
            SeedTestData(_context);

            // 3. Set up the MovieController with the MovieContext
            _controller = new MovieController(_context);
        }

        // Helper method to seed initial data into the in-memory database
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

        [TestMethod]
        public async Task GetMovies_ShouldReturnAllMovies()
        {
            // Act
            var result = await _controller.GetMovie();

            // Assert
            Assert.IsNotNull(result);                                        // Ensure the result is not null

            var movies = result.Value;                                       // Access the movies list directly from the ActionResult
            Assert.IsNotNull(movies);                                        // Ensure the movies list is not null
            Assert.AreEqual(3, movies.Count());                              // Verify that 3 movies were returned

            // Optionally, verify the details of the movies
            var moviesList = movies.ToList();
            Assert.AreEqual(1, moviesList[0].Id);
            Assert.AreEqual("Movie 1", moviesList[0].MovieTitle);
            Assert.AreEqual("Action", moviesList[0].Genre);
            Assert.AreEqual(4, moviesList[0].Rating);
        }
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
            // Verify that the result is not null
            Assert.IsNotNull(result);

            // Verify that the result is of type CreatedAtActionResult
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));

            // Cast the result to CreatedAtActionResult and verify the returned movie
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode); // Ensure it's a 201 Created status code

            // Verify the created movie object in the result matches the new movie
            var createdMovie = createdResult.Value as Movie;
            Assert.IsNotNull(createdMovie);
            Assert.AreEqual(newMovie.MovieTitle, createdMovie.MovieTitle);
            Assert.AreEqual(newMovie.Genre, createdMovie.Genre);
            Assert.AreEqual(newMovie.Rating, createdMovie.Rating);

            // Verify that the movie was actually added to the database
            var movieInDb = await _context.Movie.FindAsync(createdMovie.Id);
            Assert.IsNotNull(movieInDb);
            Assert.AreEqual(newMovie.MovieTitle, movieInDb.MovieTitle);
            Assert.AreEqual(newMovie.Genre, movieInDb.Genre);
            Assert.AreEqual(newMovie.Rating, movieInDb.Rating);
        }

        [TestMethod]
        public async Task DeleteMovie_ShouldDeleteMovieSuccessfully()
        {
            // Arrange
            int movieIdToDelete = 1; // Assume movie with ID 1 exists in the seed data

            // Act
            var result = await _controller.DeleteMovie(movieIdToDelete);

            // Assert
            // Verify that the result is not null
            Assert.IsNotNull(result);

            // Verify that the result is of type NoContentResult
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            // Verify that the movie has been removed from the database
            var movieInDb = await _context.Movie.FindAsync(movieIdToDelete);
            Assert.IsNull(movieInDb);
        }


    }
}
