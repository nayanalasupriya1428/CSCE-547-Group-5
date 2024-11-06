using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        // Method inadvertenly tests both CreateUser and GetUsers
        [TestMethod]
        public void GetUsers_ShouldReturnAllUsers()
        {
            // Arrange
            // Adding users to the list directly by interacting with the controller
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
            Assert.IsNotNull(result, "Result should not be null"); // Ensure the result is not null

            // Check if the result is of type OkObjectResult
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult), "Result should be of type OkObjectResult");

            // Cast result to OkObjectResult to access the returned value
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "OkObjectResult should not be null");

            // Verify that the returned value is the list of users
            var returnedUsers = okResult.Value as List<User>;
            Assert.IsNotNull(returnedUsers, "Returned users list should not be null");
            Assert.AreEqual(2, returnedUsers.Count, "The users list should contain exactly 2 users");

            // Optionally, verify the details of the users
            Assert.AreEqual("user1@example.com", returnedUsers[0].Email);
            Assert.AreEqual("user2@example.com", returnedUsers[1].Email);
        }
        [TestMethod]
        public void UpdateUser_ShouldUpdateExistingUserSuccessfully()
        {
            // Arrange
            // Add a user to be updated
            var newUser = new User
            {
                Email = "olduser@example.com",
                Username = "olduser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123",
                NotificationPreference = UserPreference.Email
            };
            _controller.CreateUser(newUser); // Add the user via CreateUser method

            // Create updated user details
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
            // Check if the result is of type NoContentResult
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "The result should be of type NoContentResult");

            // Verify that the user's details were updated correctly
            var getResult = _controller.GetUser(newUser.Id);

            // Ensure the result is not null and is of type OkObjectResult
            Assert.IsNotNull(getResult.Result, "The user should be found");
            Assert.IsInstanceOfType(getResult.Result, typeof(OkObjectResult), "Result should be of type OkObjectResult");

            // Cast the result to OkObjectResult to access the value
            var okResult = getResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "OkObjectResult should not be null");

            // Access the updated user from the value of OkObjectResult
            var updatedResultUser = okResult.Value as User;
            Assert.IsNotNull(updatedResultUser, "Updated user should not be null");
            Assert.AreEqual("newuser@example.com", updatedResultUser.Email);
            Assert.AreEqual("newuser", updatedResultUser.Username);
            Assert.AreEqual("Jane", updatedResultUser.FirstName);
            Assert.AreEqual("Smith", updatedResultUser.LastName);
            Assert.AreEqual("newpassword123", updatedResultUser.Password);
            Assert.AreEqual(UserPreference.SMS, updatedResultUser.NotificationPreference);
        }


        [TestMethod]
        public void UpdateUser_ShouldReturnNotFoundForNonExistentUser()
        {
            // Arrange
            // Create user data for update (user ID doesn't exist in the list)
            var updatedUser = new User
            {
                Email = "nonexistent@example.com",
                Username = "nonexistent",
                FirstName = "Non",
                LastName = "Existent",
                Password = "nonpassword",
                NotificationPreference = UserPreference.Both
            };
            var nonExistentUserId = 999; // ID that does not exist

            // Act
            var result = _controller.UpdateUser(nonExistentUserId, updatedUser);

            // Assert
            // Check if the result is of type NotFoundResult
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The result should be of type NotFoundResult");
        }
        [TestMethod]
        public void DeleteUser_ShouldDeleteUserSuccessfully()
        {
            // Arrange
            // Add a user to the list so that we can delete it
            var newUser = new User
            {
                Email = "user@example.com",
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123",
                NotificationPreference = UserPreference.Email
            };
            _controller.CreateUser(newUser); // Add the user via CreateUser method

            // Act
            var result = _controller.DeleteUser(newUser.Id);

            // Assert
            // Verify that the response is NoContent (HTTP 204)
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "The result should be of type NoContentResult");

            // Ensure the user was deleted by attempting to get the user, which should return NotFound
            var getResult = _controller.GetUser(newUser.Id);
            Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult), "The user should no longer exist and should return NotFound");
        }

        [TestMethod]
        public void DeleteUser_ShouldReturnNotFoundForNonExistentUser()
        {
            // Arrange
            // Use an ID that does not exist in the list
            var nonExistentUserId = 999; // This ID does not exist

            // Act
            var result = _controller.DeleteUser(nonExistentUserId);

            // Assert
            // Verify that the response is NotFound (HTTP 404)
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "The result should be of type NotFoundResult");
        }

    }
}
