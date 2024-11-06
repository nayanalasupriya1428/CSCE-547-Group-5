using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Models;
using MovieReviewApp.Controllers;

namespace MovieReviewApi.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Set up the controller (initialize new instance for each test)
            _controller = new UsersController();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // No need to manually clear the list since it's static and already tied to the controller.
            // A new instance of the controller will provide a fresh state.
        }

        [TestMethod]
        public void CreateUser_ShouldAddUserSuccessfully()
        {
            // Arrange
            var newUser = new User
            {
                Email = "newuser@example.com",
                Username = "newuser",
                FirstName = "Alice",
                LastName = "Johnson",
                Password = "password123",
                NotificationPreference = UserPreference.Email
            };

            // Act
            var result = _controller.CreateUser(newUser);

            // Assert
            Assert.IsNotNull(result); // Ensure result is not null
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult)); // Verify that the result is CreatedAtActionResult

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult); // Ensure cast is successful

            var createdUser = createdResult.Value as User;
            Assert.IsNotNull(createdUser); // Ensure the created user is not null

            // Verify that the user is assigned an ID and added to the list
            Assert.AreEqual(1, createdUser.Id); // Since no users exist initially, the ID should be 1
            Assert.AreEqual("newuser@example.com", createdUser.Email);
            Assert.AreEqual("newuser", createdUser.Username);
            Assert.AreEqual("Alice", createdUser.FirstName);
            Assert.AreEqual("Johnson", createdUser.LastName);
            Assert.AreEqual("password123", createdUser.Password);
            Assert.AreEqual(UserPreference.Email, createdUser.NotificationPreference);

            // Verify that the user was actually added to the users list through the controller
            var allUsersResult = _controller.GetUsers();
            var allUsers = allUsersResult.Value;
            Assert.IsNotNull(allUsers);
            Assert.AreEqual(1, allUsers.Count()); // Check if there's 1 user in the list now
            Assert.AreEqual("newuser@example.com", allUsers.First().Email);
        }
    }
}
