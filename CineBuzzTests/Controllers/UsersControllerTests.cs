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
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        /// <summary>
        /// Tests if Get() returns all users successfully.
        /// </summary>
        [TestMethod]
        public async Task Get_ReturnsAllUsers()
        {
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
                .ReturnsAsync(users);

            var result = await _controller.Get();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(users, okResult.Value);
        }

        /// <summary>
        /// Tests if Get(int id) returns the correct user for a valid user ID.
        /// </summary>
        [TestMethod]
        public async Task Get_WithValidId_ReturnsUser()
        {
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
                .ReturnsAsync(user);

            var result = await _controller.Get(userId);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(user, okResult.Value);
        }

        /// <summary>
        /// Tests if Get(int id) returns NotFound for an invalid user ID.
        /// </summary>
        [TestMethod]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            int userId = 999;

            _mockUserService
                .Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null);

            var result = await _controller.Get(userId);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Tests if Post creates a valid user and returns it.
        /// </summary>
        [TestMethod]
        public async Task Post_ValidUser_ReturnsCreatedUser()
        {
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
                .ReturnsAsync(newUser);

            var result = await _controller.Post(newUser);

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.Get), createdResult.ActionName);
            Assert.AreEqual(newUser.Id, createdResult.RouteValues["id"]);
            Assert.AreEqual(newUser, createdResult.Value);
        }

        /// <summary>
        /// Tests if Put updates a valid user and returns it.
        /// </summary>
        [TestMethod]
        public async Task Put_ValidUserIdAndData_ReturnsUpdatedUser()
        {
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
                .ReturnsAsync(updatedUser);

            var result = await _controller.Put(userId, updatedUser);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(updatedUser, okResult.Value);
        }

        /// <summary>
        /// Tests if Put returns NotFound for an invalid user ID.
        /// </summary>
        [TestMethod]
        public async Task Put_InvalidUserId_ReturnsNotFound()
        {
            int userId = 999;
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
                .ReturnsAsync((User)null);

            var result = await _controller.Put(userId, updatedUser);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Tests if Delete removes a valid user and returns NoContent.
        /// </summary>
        [TestMethod]
        public async Task Delete_ValidUserId_ReturnsNoContent()
        {
            int userId = 1;

            _mockUserService
                .Setup(service => service.DeleteUserAsync(userId))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(userId);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        /// <summary>
        /// Tests if Delete returns NotFound for an invalid user ID.
        /// </summary>
        [TestMethod]
        public async Task Delete_InvalidUserId_ReturnsNotFound()
        {
            int userId = 999;

            _mockUserService
                .Setup(service => service.DeleteUserAsync(userId))
                .Throws(new KeyNotFoundException());

            var result = await _controller.Delete(userId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
