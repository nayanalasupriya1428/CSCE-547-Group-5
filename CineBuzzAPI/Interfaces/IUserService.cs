using CineBuzzApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    // Interface for managing user-related operations.
    public interface IUserService
    {
        // Retrieves all users from the database asynchronously.
        Task<IEnumerable<User>> GetAllUsersAsync();

        // Retrieves a single user by their unique identifier asynchronously. Returns null if no user is found.
        Task<User?> GetUserByIdAsync(int id);

        // Adds a new user to the database and returns the newly added user asynchronously.
        Task<User> AddUserAsync(User user);

        // Updates an existing user's details in the database and returns the updated user asynchronously.
        // Returns null if the user to update is not found.
        Task<User?> UpdateUserAsync(int id, User user);

        // Deletes a user from the database based on their unique identifier asynchronously.
        Task DeleteUserAsync(int id);
    }
}
