using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public class UserService : IUserService
    {
        private readonly CineBuzzDbContext _context;

        public UserService(CineBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password;
            existingUser.NotificationPreference = user.NotificationPreference;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
