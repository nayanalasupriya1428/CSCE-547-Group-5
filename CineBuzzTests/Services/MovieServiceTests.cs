using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CineBuzzApi.Services
{
    [TestClass]
    public class MovieServiceTests
    {
        private MovieService _service;
        private CineBuzzDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new MovieService(_context);
        }
        [TestMethod]
        public async Task GetAllMoviesAsync_ReturnsAllMovies_WhenMoviesExist()
        {
            // Arrange - two movies already exist in the database.
            

            // Act
            var result = await _service.GetAllMoviesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        [TestMethod]
        public async Task AddMovieAsync_AddsMovieSuccessfully_WhenMovieIsValid()
        {
            // Arrange
            var newMovie = new Movie
            {
                Title = "Interstellar",
                Description = "A team of explorers travel through a wormhole in space.",
                Genres = new List<string> { "Adventure", "Drama", "Sci-Fi" }
            };

            // Act
            var result = await _service.AddMovieAsync(newMovie);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Interstellar", result.Title);
            Assert.AreEqual("A team of explorers travel through a wormhole in space.", result.Description);
            Assert.AreEqual(3, result.Genres.Count);
            Assert.AreEqual(3, await _context.Movies.CountAsync()); // Inception and Matrix in database
        }
        [TestMethod]
        public async Task RemoveMovieAsync_RemovesMovieSuccessfully_WhenMovieExists()
        {
            // Arrange
            var movieToRemove = new Movie
            {
                MovieId = 3,
                Title = "The Prestige",
                Description = "Two magicians engage in a competitive rivalry.",
                Genres = new List<string> { "Drama", "Mystery", "Sci-Fi" }
            };

            _context.Movies.Add(movieToRemove);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.RemoveMovieAsync(movieToRemove.MovieId);

            // Assert
            Assert.IsTrue(result); // Verify that the removal was successful
            var movieExists = await _context.Movies.AnyAsync(m => m.MovieId == movieToRemove.MovieId);
            Assert.IsFalse(movieExists); // Verify that the movie no longer exists in the database
        }
        [TestMethod]
        public async Task EditMovieAsync_UpdatesMovieSuccessfully_WhenMovieExists()
        {
            // Arrange
            var existingMovie = new Movie
            {
                MovieId = 3,
                Title = "Original Title",
                Description = "Original description of the movie.",
                Genres = new List<string> { "Action", "Drama" }
            };

            _context.Movies.Add(existingMovie);
            await _context.SaveChangesAsync();

            var updatedMovie = new Movie
            {
                Title = "Updated Title",
                Description = "Updated description of the movie.",
                Genres = new List<string> { "Action", "Thriller" }
            };

            // Act
            var result = await _service.EditMovieAsync(existingMovie.MovieId, updatedMovie);

            // Assert
            Assert.IsTrue(result); // Verify that the update was successful
            var movieInDb = await _context.Movies.FindAsync(existingMovie.MovieId);
            Assert.IsNotNull(movieInDb); // Ensure the movie still exists in the database
            Assert.AreEqual("Updated Title", movieInDb.Title);
            Assert.AreEqual("Updated description of the movie.", movieInDb.Description);
            Assert.AreEqual(2, movieInDb.Genres.Count);
            CollectionAssert.AreEqual(new List<string> { "Action", "Thriller" }, movieInDb.Genres);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
