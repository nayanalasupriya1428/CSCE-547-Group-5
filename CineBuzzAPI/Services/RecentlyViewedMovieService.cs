using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace CineBuzzApi.Services
{
    public class RecentlyViewedMoviesService : IRecentlyViewedMoviesService
    {
        private Stack<Movie> _recentlyViewedMovies = new Stack<Movie>();
        private const int MaxRecentlyViewed = 5;
         public RecentlyViewedMoviesService()
        {
            // Seed mock data for testing purposes
            _recentlyViewedMovies.Push(new Movie { MovieId = 1, Title = "Inception", Description = "A mind-bending thriller about dreams within dreams." });
            _recentlyViewedMovies.Push(new Movie { MovieId = 2, Title = "The Matrix", Description = "A hacker discovers reality is a simulation." });
            _recentlyViewedMovies.Push(new Movie { MovieId = 3, Title = "Interstellar", Description = "A journey through space to save humanity." });
        }

        public void AddMovieToRecentlyViewed(Movie movie)
        {
            if (_recentlyViewedMovies.Contains(movie))
            {
                return;
            }

            if (_recentlyViewedMovies.Count >= MaxRecentlyViewed)
            {
                _recentlyViewedMovies = new Stack<Movie>(_recentlyViewedMovies.Skip(1));
            }

            _recentlyViewedMovies.Push(movie);
        }

        public IEnumerable<Movie> GetRecentlyViewedMovies()
        {
            return _recentlyViewedMovies;
        }
    }
}
