// Required libraries for accessing the database and supporting asynchronous operations.
using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Defines a service for managing movie data.
    public class MovieService : IMovieService
    {
        // Database context used for database operations.
        private readonly CineBuzzDbContext _context;

        // Constructor that initializes the service with a database context.
        public MovieService(CineBuzzDbContext context)
        {
            _context = context;  // Save the passed database context to the private field.
        }

        // Gets a list of all movies from the database asynchronously.
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            // Asynchronously retrieves all movies from the database and returns them as a list.
            return await _context.Movies.ToListAsync();
        }

        // Adds a new movie to the database and saves the change asynchronously.
        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);  // Add the new movie to the database context.
            await _context.SaveChangesAsync();  // Save changes to the database.
            return movie;  // Return the added movie.
        }

        // Removes a movie by its ID from the database asynchronously and returns success status.
        public async Task<bool> RemoveMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);  // Find the movie by ID.
            if (movie == null) return false;  // If no movie is found, return false.

            _context.Movies.Remove(movie);  // Remove the found movie from the database context.
            await _context.SaveChangesAsync();  // Save changes to the database.
            return true;  // Return true to indicate successful removal.
        }
    }
}
