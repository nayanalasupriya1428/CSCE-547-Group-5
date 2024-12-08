using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;

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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new MovieService(_context);
        }

        /// <summary>
        /// Tests if GetAllMoviesAsync returns all movies when movies exist.
        /// </summary>
        [TestMethod]
        public async Task GetAllMoviesAsync_ReturnsAllMovies_WhenMoviesExist()
        {
            var result = await _service.GetAllMoviesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Tests if AddMovieAsync adds a valid movie successfully.
        /// </summary>
        [TestMethod]
        public async Task AddMovieAsync_AddsMovieSuccessfully_WhenMovieIsValid()
        {
            var newMovie = new Movie
            {
                Title = "Interstellar",
                Description = "A team of explorers travel through a wormhole in space.",
                Genres = new List<string> { "Adventure", "Drama", "Sci-Fi" }
            };

            var result = await _service.AddMovieAsync(newMovie);

            Assert.IsNotNull(result);
            Assert.AreEqual("Interstellar", result.Title);
            Assert.AreEqual("A team of explorers travel through a wormhole in space.", result.Description);
            Assert.AreEqual(3, result.Genres.Count);
            Assert.AreEqual(3, await _context.Movies.CountAsync());
        }

        /// <summary>
        /// Tests if RemoveMovieAsync removes a movie successfully when it exists.
        /// </summary>
        [TestMethod]
        public async Task RemoveMovieAsync_RemovesMovieSuccessfully_WhenMovieExists()
        {
            var movieToRemove = new Movie
            {
                MovieId = 3,
                Title = "The Prestige",
                Description = "Two magicians engage in a competitive rivalry.",
                Genres = new List<string> { "Drama", "Mystery", "Sci-Fi" }
            };

            _context.Movies.Add(movieToRemove);
            await _context.SaveChangesAsync();

            var result = await _service.RemoveMovieAsync(movieToRemove.MovieId);

            Assert.IsTrue(result);
            var movieExists = await _context.Movies.AnyAsync(m => m.MovieId == movieToRemove.MovieId);
            Assert.IsFalse(movieExists);
        }

        /// <summary>
        /// Tests if EditMovieAsync updates a movie successfully when it exists.
        /// </summary>
        [TestMethod]
        public async Task EditMovieAsync_UpdatesMovieSuccessfully_WhenMovieExists()
        {
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

            var result = await _service.EditMovieAsync(existingMovie.MovieId, updatedMovie);

            Assert.IsTrue(result);
            var movieInDb = await _context.Movies.FindAsync(existingMovie.MovieId);
            Assert.IsNotNull(movieInDb);
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
