using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly CineBuzzDbContext _context;

        public MovieService(CineBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> RemoveMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Movie?> EditMovieAsync(int movieId, Movie newMovie)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) 
                return null;
            // replace old movie content with new movie content
            movie.Title = newMovie.Title;
            movie.Description = newMovie.Description;
            for (int i = 0; i < movie.Genres.Count; i++)
            {
                movie.Genres[i] = newMovie.Genres[i];
            }
            // apply changes
            await _context.SaveChangesAsync();
            return movie;

        }
    }
}
