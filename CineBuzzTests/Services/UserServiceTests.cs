using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CineBuzzApi.Services
{
    /// <summary>
    /// Unit tests for the UserService class.
    /// Validates CRUD operations for users, ensuring correct behavior in various scenarios.
    /// </summary>
    [TestClass]
    public class UserServiceTests
    {
        private UserService _service;
        private CineBuzzDbContext _context;

        /// <summary>
        /// Sets up an in-memory database and initializes the UserService for testing.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new UserService(_context);
        }

        /// <summary>
        /// Cleans up the in-memory database after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        /// <summary>
        /// Tests if GetAllUsersAsync returns all users when they exist in the database.
        /// </summary>
        [TestMethod]
        public async Task GetAllUsersAsync_ReturnsAllUsers_WhenUsersExist()
        {
            var users = new List<User>
            {
                new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" },
                new User { Id = 2, Email = "jane.doe@example.com", Username = "Janedoe", FirstName = "Jane", LastName = "Doe", Password = "23456788" }
            };
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllUsersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Tests if GetUserByIdAsync returns the correct user when it exists.
        /// </summary>
        [TestMethod]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            var user = new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.GetUserByIdAsync(user.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Email, result.Email);
        }

        /// <summary>
        /// Tests if AddUserAsync successfully adds a valid user.
        /// </summary>
        [TestMethod]
        public async Task AddUserAsync_AddsUserSuccessfully_WhenUserIsValid()
        {
            var newUser = new User { Email = "alice.wonderland@example.com", Username = "AliceW", FirstName = "Alice", LastName = "Wonderland", Password = "password123" };

            var result = await _service.AddUserAsync(newUser);

            Assert.IsNotNull(result);
            Assert.AreEqual(newUser.Email, result.Email);
            Assert.AreEqual(1, await _context.Users.CountAsync());
        }

        /// <summary>
        /// Tests if UpdateUserAsync successfully updates user details when the user exists.
        /// </summary>
        [TestMethod]
        public async Task UpdateUserAsync_UpdatesUserSuccessfully_WhenUserExists()
        {
            var existingUser = new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var updatedUser = new User { Email = "john.doe@newdomain.com", Username = "JohnDoeNew", FirstName = "John", LastName = "Doe", Password = "newpassword123" };

            var result = await _service.UpdateUserAsync(existingUser.Id, updatedUser);

            Assert.IsNotNull(result);
            Assert.AreEqual("john.doe@newdomain.com", result.Email);
            Assert.AreEqual("JohnDoeNew", result.Username);
        }

        /// <summary>
        /// Tests if DeleteUserAsync successfully deletes a user when they exist.
        /// </summary>
        [TestMethod]
        public async Task DeleteUserAsync_DeletesUserSuccessfully_WhenUserExists()
        {
            var userToDelete = new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" };
            _context.Users.Add(userToDelete);
            await _context.SaveChangesAsync();

            await _service.DeleteUserAsync(userToDelete.Id);

            var deletedUser = await _context.Users.FindAsync(userToDelete.Id);
            Assert.IsNull(deletedUser);
            Assert.AreEqual(0, await _context.Users.CountAsync());
        }
    }
}
