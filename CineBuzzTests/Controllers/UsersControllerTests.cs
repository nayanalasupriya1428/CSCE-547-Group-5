using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineBuzzTests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UsersController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the mock service
            _mockUserService = new Mock<IUserService>();

            // Instantiate the controller with the mock service
            _controller = new UsersController(_mockUserService.Object);
        }

        [TestMethod]
        public async Task Get_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
    {
        new User
        {
            Id = 1,
            Email = "john.doe@example.com",
            Username = "johndoe",
            FirstName = "John",
            LastName = "Doe",
            Password = "password123"
        },
        new User
        {
            Id = 2,
            Email = "jane.doe@example.com",
            Username = "janedoe",
            FirstName = "Jane",
            LastName = "Doe",
            Password = "securepassword"
        }
    };

            _mockUserService
                .Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(users); // Mock the service to return the list of users

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(users, okResult.Value);
        }
        [TestMethod]
        public async Task Get_WithValidId_ReturnsUser()
        {
            // Arrange
            int userId = 1;
            var user = new User
            {
                Id = userId,
                Email = "john.doe@example.com",
                Username = "johndoe",
                FirstName = "John",
                LastName = "Doe",
                Password = "password123"
            };

            _mockUserService
                .Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(user); // Mock the service to return the user for the valid ID

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(user, okResult.Value);
        }

        [TestMethod]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int userId = 999; // Nonexistent user ID

            _mockUserService
                .Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null); // Mock the service to return null for the invalid ID

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Post_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var newUser = new User
            {
                Id = 1,
                Email = "new.user@example.com",
                Username = "newuser",
                FirstName = "New",
                LastName = "User",
                Password = "password123"
            };

            _mockUserService
                .Setup(service => service.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(newUser); // Mock the service to return the created user

            // Act
            var result = await _controller.Post(newUser);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.Get), createdResult.ActionName); // Ensure the action name matches "Get"
            Assert.AreEqual(newUser.Id, createdResult.RouteValues["id"]); // Ensure the route value for ID matches
            Assert.AreEqual(newUser, createdResult.Value); // Ensure the returned value matches the created user
        }
        [TestMethod]
        public async Task Put_ValidUserIdAndData_ReturnsUpdatedUser()
        {
            // Arrange
            int userId = 1;
            var updatedUser = new User
            {
                Id = userId,
                Email = "updated.user@example.com",
                Username = "updateduser",
                FirstName = "Updated",
                LastName = "User",
                Password = "newpassword123"
            };

            _mockUserService
                .Setup(service => service.UpdateUserAsync(userId, updatedUser))
                .ReturnsAsync(updatedUser); // Mock the service to return the updated user

            // Act
            var result = await _controller.Put(userId, updatedUser);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(updatedUser, okResult.Value);
        }

        [TestMethod]
        public async Task Put_InvalidUserId_ReturnsNotFound()
        {
            // Arrange
            int userId = 999; // Nonexistent user ID
            var updatedUser = new User
            {
                Id = userId,
                Email = "updated.user@example.com",
                Username = "updateduser",
                FirstName = "Updated",
                LastName = "User",
                Password = "newpassword123"
            };

            _mockUserService
                .Setup(service => service.UpdateUserAsync(userId, updatedUser))
                .ReturnsAsync((User)null); // Mock the service to return null for an invalid ID

            // Act
            var result = await _controller.Put(userId, updatedUser);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Delete_ValidUserId_ReturnsNoContent()
        {
            // Arrange
            int userId = 1;

            _mockUserService
                .Setup(service => service.DeleteUserAsync(userId))
                .Returns(Task.CompletedTask); // Mock the service to simulate successful deletion

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_InvalidUserId_ReturnsNotFound()
        {
            // Arrange
            int userId = 999; // Nonexistent user ID

            _mockUserService
                .Setup(service => service.DeleteUserAsync(userId))
                .Throws(new KeyNotFoundException()); // Mock the service to throw KeyNotFoundException for invalid ID

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


    }
}
