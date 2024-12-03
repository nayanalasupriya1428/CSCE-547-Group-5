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
    [TestClass]
    public class UserServiceTests
    {
        private UserService _service;
        private CineBuzzDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new UserService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetAllUsersAsync_ReturnsAllUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" },
                new User { Id = 2, Email = "jane.doe@example.com", Username = "Janedoe", FirstName = "Jane", LastName = "Doe", Password = "23456788" }
            };
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Should return 2 users
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetUserByIdAsync(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public async Task AddUserAsync_AddsUserSuccessfully_WhenUserIsValid()
        {
            // Arrange
            var newUser = new User { Email = "alice.wonderland@example.com", Username = "AliceW", FirstName = "Alice", LastName = "Wonderland", Password = "password123" };

            // Act
            var result = await _service.AddUserAsync(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newUser.Email, result.Email);
            Assert.AreEqual(1, await _context.Users.CountAsync()); // Ensure that the user is added to the database
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdatesUserSuccessfully_WhenUserExists()
        {
            // Arrange
            var existingUser = new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var updatedUser = new User { Email = "john.doe@newdomain.com", Username = "JohnDoeNew", FirstName = "John", LastName = "Doe", Password = "newpassword123" };

            // Act
            var result = await _service.UpdateUserAsync(existingUser.Id, updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("john.doe@newdomain.com", result.Email);
            Assert.AreEqual("JohnDoeNew", result.Username);
        }

        [TestMethod]
        public async Task DeleteUserAsync_DeletesUserSuccessfully_WhenUserExists()
        {
            // Arrange
            var userToDelete = new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" };
            _context.Users.Add(userToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteUserAsync(userToDelete.Id);

            // Assert
            var deletedUser = await _context.Users.FindAsync(userToDelete.Id);
            Assert.IsNull(deletedUser); // Verify that the user is deleted
            Assert.AreEqual(0, await _context.Users.CountAsync()); // Ensure no users remain
        }
    }
}
