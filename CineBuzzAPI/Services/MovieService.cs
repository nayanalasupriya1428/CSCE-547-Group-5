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

        public async Task<bool> EditMovieAsync(int movieId, Movie movie)
        {
            var oldMovie = await _context.Movies.FindAsync(movieId);
            if (oldMovie == null) return false;

            // Update the properties of the existing movie with new values.
            oldMovie.Title = movie.Title;
            oldMovie.Description = movie.Description;

            // Movie has a list of genres. We need to update the genres, but how?
            // There are cases that can arise from this operation.
            // 1. The new Movie is adding genres that the old Movie did not have.
            // 2. The new Movie is replacing the genres of the old Movie.
            // For now, I will assume that the new Movie is replacing the genres of the old Movie.

            // Remove all genres from the old movie.
            oldMovie.Genres.Clear();

            // Add all genres from the new movie.
            foreach (var genre in movie.Genres)
            {
                oldMovie.Genres.Add(genre);
            }

            return await _context.SaveChangesAsync() > 0;


        }
    }
}
