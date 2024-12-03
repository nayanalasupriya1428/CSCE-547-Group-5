using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace CineBuzzApi.Services
{
    // Manages the list of recently viewed movies.
    public class RecentlyViewedMoviesService : IRecentlyViewedMoviesService
    {
        // Stack to store recently viewed movies.
        private Queue<Movie> _recentlyViewedMovies = new Queue<Movie>();
        // Maximum number of recently viewed movies to keep track of.
        private const int MaxRecentlyViewed = 5;

        // Constructor to initialize with some mock data for testing.
        public RecentlyViewedMoviesService()
        {
            // Adding mock movies to the stack for initial data.
            _recentlyViewedMovies.Enqueue(new Movie { MovieId = 1, Title = "Inception", Description = "A mind-bending thriller about dreams within dreams." });
            _recentlyViewedMovies.Enqueue(new Movie { MovieId = 2, Title = "The Matrix", Description = "A hacker discovers reality is a simulation." });
            _recentlyViewedMovies.Enqueue(new Movie { MovieId = 3, Title = "Interstellar", Description = "A journey through space to save humanity." });
        }

        // Adds a movie to the stack of recently viewed movies, ensuring no duplicates.
        public void AddMovieToRecentlyViewed(Movie movie)
        {
            //// Check if the movie is already in the stack to avoid duplicates.
            //if (_recentlyViewedMovies.Contains(movie))
            //{
            //    return;  // If it's already there, do nothing.
            //}

            //// If the stack is full, remove the oldest movie.
            //if (_recentlyViewedMovies.Count >= MaxRecentlyViewed)
            //{
            //    // Create a new stack without the oldest movie.
            //    _recentlyViewedMovies = new Stack<Movie>(_recentlyViewedMovies.Skip(1));
            //}

            //// Add the new movie to the stack.
            //_recentlyViewedMovies.Push(movie);

            // If there is a movie that exists in the recent movies queue, return.
            if (_recentlyViewedMovies.Contains(movie))
            {
                return;
            }

            // If the queue is full, remove the oldest movie.
            if (_recentlyViewedMovies.Count >= MaxRecentlyViewed)
            {
                _recentlyViewedMovies.Dequeue();
            }

            // Add the new movie to the queue.
            _recentlyViewedMovies.Enqueue(movie);


        }

        // Returns all movies in the stack of recently viewed movies.
        public IEnumerable<Movie> GetRecentlyViewedMovies()
        {
            return _recentlyViewedMovies;
        }
    }
}
