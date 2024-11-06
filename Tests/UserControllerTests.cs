// @author Scott Do (Reshlynt)
// Unit tests for CartController class in MovieReviewApi.
// @date 2024-11-5
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
            // Set up the controller with a new instance for each test
            _controller = new UsersController();
        }

        /// <summary>
        /// Tests that retrieving all users returns the correct list of users.
        /// </summary>
        [TestMethod]
        public void GetUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var user1 = new User
            {
                Email = "user1@example.com",
                Username = "user1",
                FirstName = "John",
                LastName = "Doe",
                Password = "password1",
                NotificationPreference = UserPreference.Email
            };
            _controller.CreateUser(user1);

            var user2 = new User
            {
                Email = "user2@example.com",
                Username = "user2",
                FirstName = "Jane",
                LastName = "Doe",
                Password = "password2",
                NotificationPreference = UserPreference.SMS
            };
            _controller.CreateUser(user2);

            // Act
            var result = _controller.GetUsers();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult), "Result should be of type OkObjectResult");

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "OkObjectResult should not be null");

            var returnedUsers = okResult.Value as List<User>;
            Assert.IsNotNull(returnedUsers, "Returned users list should not be null");
            Assert.AreEqual(2, returnedUsers.Count, "The users list should contain exactly 2 users");

            Assert.AreEqual("user1@example.com", returnedUsers[0].Email);
            Assert.AreEqual("user2@example.com", returnedUsers[1].Email);
        }

        /// <summary>
        /// Tests that updating an existing user returns a successful result.
        /// </summary>
        [TestMethod]
        public void UpdateUser_ShouldUpdateExistingUserSuccessfully()
        {
            // Arrange
            var newUser = new User
            {
                Email = "olduser@example.com",
                Username = "olduser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123",
                NotificationPreference = UserPreference.Email
            };
            _controller.CreateUser(newUser);

            var updatedUser = new User
            {
                Email = "newuser@example.com",
                Username = "newuser",
                FirstName = "Jane",
                LastName = "Smith",
                Password = "newpassword123",
                NotificationPreference = UserPreference.SMS
            };

            // Act
            var result = _controller.UpdateUser(newUser.Id, updatedUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "The result should be of type NoContentResult");

            var getResult = _controller.GetUser(newUser.Id);
            Assert.IsNotNull(getResult.Result, "The user should be found");
            Assert.IsInstanceOfType(getResult.Result, typeof(OkObjectResult), "Result should be of type OkObjectResult");

            var okResult = getResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "OkObjectResult should not be null");

            var updatedResultUser = okResult.Value as User;
            Assert.IsNotNull(updatedResultUser, "Updated user should not be null");
            Assert.AreEqual("newuser@example.com", updatedResultUser.Email);
            Assert.AreEqual("newuser", updatedResultUser.Username);
            Assert.AreEqual("Jane", updatedResultUser.FirstName);
            Assert.AreEqual("Smith", updatedResultUser.LastName);
            Assert.AreEqual("newpassword123", updatedResultUser.Password);
            Assert.AreEqual(UserPreference.SMS, updatedResultUser.NotificationPreference);
        }

        /// <summary>
        /// Tests that updating a non-existent user returns a NotFound result.
        /// </summary>
        [TestMethod]
        public void UpdateUser_ShouldReturnNotFoundForNonExistentUser()
        {
            // Arrange
            var updatedUser = new User
            {
                Email = "nonexistent@example.com",
                Username = "nonexistent",
                FirstName = "Non",
                LastName = "Existent",
                Password = "nonpassword",
                NotificationPreference = UserPreference.Both
            };
            var nonExistentUserId = 999;

            // Act
            var result = _controller.UpdateUser(nonExistentUserId, updatedUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The result should be of type NotFoundResult");
        }

        /// <summary>
        /// Tests that deleting an existing user returns a successful result.
        /// </summary>
        [TestMethod]
        public void DeleteUser_ShouldDeleteUserSuccessfully()
        {
            // Arrange
            var newUser = new User
            {
                Email = "user@example.com",
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123",
                NotificationPreference = UserPreference.Email
            };
            _controller.CreateUser(newUser);

            // Act
            var result = _controller.DeleteUser(newUser.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "The result should be of type NoContentResult");

            var getResult = _controller.GetUser(newUser.Id);
            Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult), "The user should no longer exist and should return NotFound");
        }

        /// <summary>
        /// Tests that attempting to delete a non-existent user returns a NotFound result.
        /// </summary>
        [TestMethod]
        public void DeleteUser_ShouldReturnNotFoundForNonExistentUser()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act
            var result = _controller.DeleteUser(nonExistentUserId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The result should be of type NotFoundResult");
        }
    }
}
