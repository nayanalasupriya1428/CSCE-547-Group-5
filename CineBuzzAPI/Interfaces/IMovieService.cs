using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Interface for managing movie-related operations.
    public interface IMovieService
    {
        // Retrieves a list of all movies from the database asynchronously.
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        // Adds a new movie to the database and returns the added movie asynchronously.
        Task<Movie> AddMovieAsync(Movie movie);

        // Removes a movie from the database based on the movie ID. Returns true if the removal was successful.
        Task<bool> RemoveMovieAsync(int movieId);

        // Edits the content of a movie with new movie detail. Returns a bool on operation status.
        Task<bool> EditMovieAsync(int movieId, Movie movie);
    }
}

