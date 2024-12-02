using CineBuzzApi.Models;
using System.Collections.Generic;

namespace CineBuzzApi.Services
{
    // Interface for managing the list of recently viewed movies.
    public interface IRecentlyViewedMoviesService
    {
        // Adds a movie to the list of recently viewed movies.
        void AddMovieToRecentlyViewed(Movie movie);

        // Retrieves the list of movies that have been recently viewed.
        IEnumerable<Movie> GetRecentlyViewedMovies();
    }
}

