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
            _mockTicketService = new Mock<ITicketService>();
            _controller = new TicketsController(_mockTicketService.Object);
        }

        /// <summary>
        /// Tests if Get() returns all tickets successfully.
        /// </summary>
        [TestMethod]
        public async Task Get_ReturnsAllTickets()
        {
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieTimeId = 101, Price = 10.50, Quantity = 2, Availability = true, SeatNumber = 5 },
                new Ticket { TicketId = 2, MovieTimeId = 102, Price = 12.00, Quantity = 1, Availability = true, SeatNumber = 8 }
            };

            _mockTicketService
                .Setup(service => service.GetAllTicketsAsync())
                .ReturnsAsync(tickets);

            var result = await _controller.Get();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(tickets, okResult.Value);
        }

        /// <summary>
        /// Tests if Get(int id) returns the correct ticket for a valid ID.
        /// </summary>
        [TestMethod]
        public async Task Get_WithValidId_ReturnsTicket()
        {
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
                .ReturnsAsync(ticket);

            var result = await _controller.Get(ticketId);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(ticket, okResult.Value);
        }

        /// <summary>
        /// Tests if Get(int id) returns NotFound for an invalid ID.
        /// </summary>
        [TestMethod]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            int ticketId = 999;

            _mockTicketService
                .Setup(service => service.GetTicketByIdAsync(ticketId))
                .ReturnsAsync((Ticket)null);

            var result = await _controller.Get(ticketId);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Tests if Post creates a valid ticket and returns it.
        /// </summary>
        [TestMethod]
        public async Task Post_ValidTicket_ReturnsCreatedTicket()
        {
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
                .ReturnsAsync(ticket);

            var result = await _controller.Post(ticket);

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.Get), createdResult.ActionName);
            Assert.AreEqual(ticket.TicketId, createdResult.RouteValues["id"]);
            Assert.AreEqual(ticket, createdResult.Value);
        }

        /// <summary>
        /// Tests if Put updates a ticket successfully for a valid ID and data.
        /// </summary>
        [TestMethod]
        public async Task Put_ValidTicketIdAndData_ReturnsUpdatedTicket()
        {
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
                .ReturnsAsync(updatedTicket);

            var result = await _controller.Put(ticketId, updatedTicket);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(updatedTicket, okResult.Value);
        }

        /// <summary>
        /// Tests if Put returns NotFound for an invalid ticket ID.
        /// </summary>
        [TestMethod]
        public async Task Put_InvalidTicketId_ReturnsNotFound()
        {
            int ticketId = 999;
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
                .ReturnsAsync((Ticket)null);

            var result = await _controller.Put(ticketId, updatedTicket);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        /// <summary>
        /// Tests if Delete removes a ticket successfully for a valid ID.
        /// </summary>
        [TestMethod]
        public async Task Delete_ValidTicketId_ReturnsNoContent()
        {
            int ticketId = 1;

            _mockTicketService
                .Setup(service => service.DeleteTicketAsync(ticketId))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(ticketId);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        /// <summary>
        /// Tests if Delete returns NotFound for an invalid ticket ID.
        /// </summary>
        [TestMethod]
        public async Task Delete_InvalidTicketId_ReturnsNotFound()
        {
            int ticketId = 999;

            _mockTicketService
                .Setup(service => service.DeleteTicketAsync(ticketId))
                .Throws(new KeyNotFoundException());

            var result = await _controller.Delete(ticketId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
