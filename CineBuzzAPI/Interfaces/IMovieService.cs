using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie> AddMovieAsync(Movie movie);
        Task<bool> RemoveMovieAsync(int movieId);

        // put action, edit any movie from the database. arguments are the old movie id, and the New Movie object
        //Task<Movie> EditMovieAsync(int movieId, Movie newMovie);
    }
}

