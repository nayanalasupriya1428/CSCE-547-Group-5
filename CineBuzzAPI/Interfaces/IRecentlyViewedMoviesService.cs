using CineBuzzApi.Models;
using System.Collections.Generic;

namespace CineBuzzApi.Services
{
    public interface IRecentlyViewedMoviesService
    {
        void AddMovieToRecentlyViewed(Movie movie);
        IEnumerable<Movie> GetRecentlyViewedMovies();
    }
}
