using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Service class for handling user data operations.
    public class UserService : IUserService
    {
        // Database context for data operations.
        private readonly CineBuzzDbContext _context;

        // Constructor initializes the service with a database context.
        public UserService(CineBuzzDbContext context)
        {
            _context = context;  // Set the database context.
        }

        // Retrieves all users from the database.
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Asynchronously get the list of all users.
            return await _context.Users.ToListAsync();
        }

        // Retrieves a single user by their ID.
        public async Task<User?> GetUserByIdAsync(int id)
        {
            // Asynchronously find a user by their ID.
            return await _context.Users.FindAsync(id);
        }

        // Adds a new user to the database and saves the change.
        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);  // Add the new user to the database.
            await _context.SaveChangesAsync();  // Save the database changes.
            return user;  // Return the newly added user.
        }

        // Updates an existing user's details.
        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);  // Find the existing user by ID.
            if (existingUser == null) return null;  // If no user is found, return null.

            // Update user details.
            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password;

            await _context.SaveChangesAsync();  // Save the updated details to the database.
            return existingUser;  // Return the updated user.
        }

        // Deletes a user from the database.
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);  // Find the user by ID.
            if (user != null)  // If the user is found,
            {
                _context.Users.Remove(user);  // Remove the user from the database.
                await _context.SaveChangesAsync();  // Save the change to the database.
            }
        }
    }
}
