using Microsoft.VisualStudio.TestTools.UnitTesting;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Linq;


namespace CineBuzzApi.Services
{
    [TestClass]
    public class RecentlyViewedMoviesServiceTests
    {
        private RecentlyViewedMoviesService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new RecentlyViewedMoviesService();
        }

        /// <summary>
        /// Tests if AddMovieToRecentlyViewed adds a new movie when it is not already in the stack.
        /// </summary>
        [TestMethod]
        public void AddMovieToRecentlyViewed_AddsNewMovie_WhenMovieIsNotInStack()
        {
            var newMovie = new Movie { MovieId = 4, Title = "The Dark Knight", Description = "A superhero film about Batman." };

            _service.AddMovieToRecentlyViewed(newMovie);
            var result = _service.GetRecentlyViewedMovies();

            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(newMovie.MovieId, result.Last().MovieId);
        }

        /// <summary>
        /// Tests if AddMovieToRecentlyViewed does not add a duplicate movie if it is already in the stack.
        /// </summary>
        [TestMethod]
        public void AddMovieToRecentlyViewed_DoesNotAddDuplicate_WhenMovieIsAlreadyInStack()
        {
            var existingMovie = new Movie { MovieId = 1, Title = "Inception", Description = "A mind-bending thriller about dreams within dreams." };

            _service.AddMovieToRecentlyViewed(existingMovie);
            var result = _service.GetRecentlyViewedMovies();

            Assert.AreEqual(3, result.Count());
        }

        /// <summary>
        /// Tests if AddMovieToRecentlyViewed removes the oldest movie when the maximum limit is reached.
        /// </summary>
        [TestMethod]
        public void AddMovieToRecentlyViewed_RemovesOldestMovie_WhenMaxLimitIsReached()
        {
            var newMovie1 = new Movie { MovieId = 4, Title = "The Dark Knight", Description = "A superhero film about Batman." };
            var newMovie2 = new Movie { MovieId = 5, Title = "Avatar", Description = "A journey to an alien world." };
            var newMovie3 = new Movie { MovieId = 6, Title = "Gladiator", Description = "A Roman general seeks revenge." };

            _service.AddMovieToRecentlyViewed(newMovie1);
            _service.AddMovieToRecentlyViewed(newMovie2);
            _service.AddMovieToRecentlyViewed(newMovie3);
            var result = _service.GetRecentlyViewedMovies();

            Assert.AreEqual(5, result.Count());
            Assert.IsFalse(result.Any(m => m.MovieId == 1)); // The oldest movie (with MovieId 1) should be removed
            Assert.AreEqual(newMovie3.MovieId, result.Last().MovieId);
        }
    }
}
