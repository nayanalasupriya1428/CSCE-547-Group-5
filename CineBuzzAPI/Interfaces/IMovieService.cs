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
    }
}

