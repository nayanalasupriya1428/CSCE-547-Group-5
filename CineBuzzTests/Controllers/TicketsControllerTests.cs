using CineBuzzApi.Controllers;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CineBuzzTests.Controllers
{
    [TestClass]
    public class TicketsControllerTests
    {
        private Mock<ITicketService> _mockTicketService;
        private TicketsController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the mock service
            _mockTicketService = new Mock<ITicketService>();

            // Instantiate the controller with the mock service
            _controller = new TicketsController(_mockTicketService.Object);
        }

        [TestMethod]
        public async Task Get_ReturnsAllTickets()
        {
            // Arrange
            var tickets = new List<Ticket>
    {
        new Ticket { TicketId = 1, MovieTimeId = 101, Price = 10.50, Quantity = 2, Availability = true, SeatNumber = 5 },
        new Ticket { TicketId = 2, MovieTimeId = 102, Price = 12.00, Quantity = 1, Availability = true, SeatNumber = 8 }
    };

            _mockTicketService
                .Setup(service => service.GetAllTicketsAsync())
                .ReturnsAsync(tickets); // Mock the service to return the list of tickets

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(tickets, okResult.Value);
        }
        [TestMethod]
        public async Task Get_WithValidId_ReturnsTicket()
        {
            // Arrange
            int ticketId = 1;
            var ticket = new Ticket
            {
                TicketId = ticketId,
                MovieTimeId = 101,
                Price = 10.50,
                Quantity = 2,
                Availability = true,
                SeatNumber = 5
            };

            _mockTicketService
                .Setup(service => service.GetTicketByIdAsync(ticketId))
                .ReturnsAsync(ticket); // Mock the service to return the ticket for the valid ID

            // Act
            var result = await _controller.Get(ticketId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(ticket, okResult.Value);
        }

        [TestMethod]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int ticketId = 999; // Nonexistent ticket ID

            _mockTicketService
                .Setup(service => service.GetTicketByIdAsync(ticketId))
                .ReturnsAsync((Ticket)null); // Mock the service to return null for the invalid ID

            // Act
            var result = await _controller.Get(ticketId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Post_ValidTicket_ReturnsCreatedTicket()
        {
            // Arrange
            var ticket = new Ticket
            {
                TicketId = 1,
                MovieTimeId = 101,
                Price = 10.50,
                Quantity = 2,
                Availability = true,
                SeatNumber = 5
            };

            _mockTicketService
                .Setup(service => service.AddTicketAsync(ticket))
                .ReturnsAsync(ticket); // Mock the service to return the created ticket

            // Act
            var result = await _controller.Post(ticket);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.Get), createdResult.ActionName); // Ensure the action name matches "Get"
            Assert.AreEqual(ticket.TicketId, createdResult.RouteValues["id"]); // Ensure the route value for ID matches
            Assert.AreEqual(ticket, createdResult.Value); // Ensure the returned value matches the created ticket
        }
        [TestMethod]
        public async Task Put_ValidTicketIdAndData_ReturnsUpdatedTicket()
        {
            // Arrange
            int ticketId = 1;
            var updatedTicket = new Ticket
            {
                TicketId = ticketId,
                MovieTimeId = 101,
                Price = 12.00,
                Quantity = 1,
                Availability = true,
                SeatNumber = 5
            };

            _mockTicketService
                .Setup(service => service.UpdateTicketAsync(ticketId, updatedTicket))
                .ReturnsAsync(updatedTicket); // Mock the service to return the updated ticket

            // Act
            var result = await _controller.Put(ticketId, updatedTicket);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(updatedTicket, okResult.Value);
        }

        [TestMethod]
        public async Task Put_InvalidTicketId_ReturnsNotFound()
        {
            // Arrange
            int ticketId = 999; // Nonexistent ticket ID
            var updatedTicket = new Ticket
            {
                TicketId = ticketId,
                MovieTimeId = 101,
                Price = 12.00,
                Quantity = 1,
                Availability = true,
                SeatNumber = 5
            };

            _mockTicketService
                .Setup(service => service.UpdateTicketAsync(ticketId, updatedTicket))
                .ReturnsAsync((Ticket)null); // Mock the service to return null for an invalid ID

            // Act
            var result = await _controller.Put(ticketId, updatedTicket);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Delete_ValidTicketId_ReturnsNoContent()
        {
            // Arrange
            int ticketId = 1;

            _mockTicketService
                .Setup(service => service.DeleteTicketAsync(ticketId))
                .Returns(Task.CompletedTask); // Mock the service to successfully delete the ticket

            // Act
            var result = await _controller.Delete(ticketId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_InvalidTicketId_ReturnsNotFound()
        {
            // Arrange
            int ticketId = 999; // Nonexistent ticket ID

            _mockTicketService
                .Setup(service => service.DeleteTicketAsync(ticketId))
                .Throws(new KeyNotFoundException()); // Mock the service to throw exception for invalid ID

            // Act
            var result = await _controller.Delete(ticketId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

    }
}
