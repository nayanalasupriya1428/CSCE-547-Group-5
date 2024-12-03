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
        [TestMethod]
        public void AddMovieToRecentlyViewed_AddsNewMovie_WhenMovieIsNotInStack()
        {
            // Arrange
            var newMovie = new Movie { MovieId = 4, Title = "The Dark Knight", Description = "A superhero film about Batman." };

            // Act
            _service.AddMovieToRecentlyViewed(newMovie);
            var result = _service.GetRecentlyViewedMovies();

            // Assert
            Assert.AreEqual(4, result.Count()); // Should now contain 4 movies
            Assert.AreEqual(newMovie.MovieId, result.Last().MovieId);
        }
        [TestMethod]
        public void AddMovieToRecentlyViewed_DoesNotAddDuplicate_WhenMovieIsAlreadyInStack()
        {
            // Arrange
            var existingMovie = new Movie { MovieId = 1, Title = "Inception", Description = "A mind-bending thriller about dreams within dreams." };

            // Act
            _service.AddMovieToRecentlyViewed(existingMovie);
            var result = _service.GetRecentlyViewedMovies();

            // Assert
            Assert.AreEqual(3, result.Count()); // Count should still be 3, no duplicates added
        }
        [TestMethod]
        public void AddMovieToRecentlyViewed_RemovesOldestMovie_WhenMaxLimitIsReached()
        {
            // Arrange
            var newMovie1 = new Movie { MovieId = 4, Title = "The Dark Knight", Description = "A superhero film about Batman." };
            var newMovie2 = new Movie { MovieId = 5, Title = "Avatar", Description = "A journey to an alien world." };
            var newMovie3 = new Movie { MovieId = 6, Title = "Gladiator", Description = "A Roman general seeks revenge." };

            // Act
            _service.AddMovieToRecentlyViewed(newMovie1);
            _service.AddMovieToRecentlyViewed(newMovie2);
            _service.AddMovieToRecentlyViewed(newMovie3);
            var result = _service.GetRecentlyViewedMovies();

            // Assert
            Assert.AreEqual(5, result.Count()); // The stack should only contain 5 movies
            Assert.IsFalse(result.Any(m => m.MovieId == 1)); // The oldest movie (with MovieId 1) should be removed
            Assert.AreEqual(newMovie3.MovieId, result.Last().MovieId); // The newest movie should be on top
        }
    }
}
