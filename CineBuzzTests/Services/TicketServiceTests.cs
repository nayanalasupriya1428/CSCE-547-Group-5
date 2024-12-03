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
    public class TicketServiceTests
    {
        private TicketService _service;
        private CineBuzzDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new TicketService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsAllTickets_WhenTicketsExist()
        {
            // Arrange
            _context.RemoveRange(_context.Tickets);
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 },
                new Ticket { TicketId = 2, MovieTimeId = 1, Price = 15.0, Quantity = 30, Availability = true, SeatNumber = 102 }
            };
            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllTicketsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Should return 2 tickets
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_ReturnsTicket_WhenTicketExists()
        {
            // Arrange
            _context.RemoveRange(_context.Tickets);
            var ticket = new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetTicketByIdAsync(ticket.TicketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.TicketId, result.TicketId);
            Assert.AreEqual(ticket.Price, result.Price);
        }

        [TestMethod]
        public async Task AddTicketAsync_AddsTicketSuccessfully_WhenTicketIsValid()
        {
            // Arrange
            _context.RemoveRange(_context.Tickets);
            var newTicket = new Ticket { MovieTimeId = 1, Price = 12.0, Quantity = 25, Availability = true, SeatNumber = 103 };

            // Act
            var result = await _service.AddTicketAsync(newTicket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newTicket.Price, result.Price);
            Assert.AreEqual(1, await _context.Tickets.CountAsync()); // Ensure that the ticket is added to the database
        }

        [TestMethod]
        public async Task UpdateTicketAsync_UpdatesTicketSuccessfully_WhenTicketExists()
        {
            // Arrange
            _context.RemoveRange(_context.Tickets);
            var existingTicket = new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 };
            _context.Tickets.Add(existingTicket);
            await _context.SaveChangesAsync();

            var updatedTicket = new Ticket { Price = 15.0, Quantity = 40, Availability = false, SeatNumber = 101, MovieTimeId = 1 };

            // Act
            var result = await _service.UpdateTicketAsync(existingTicket.TicketId, updatedTicket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(15.0, result.Price);
            Assert.AreEqual(40, result.Quantity);
            Assert.IsFalse(result.Availability);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_DeletesTicketSuccessfully_WhenTicketExists()
        {
            // Arrange
            _context.RemoveRange(_context.Tickets);
            var ticketToDelete = new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 };
            _context.Tickets.Add(ticketToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteTicketAsync(ticketToDelete.TicketId);

            // Assert
            var deletedTicket = await _context.Tickets.FindAsync(ticketToDelete.TicketId);
            Assert.IsNull(deletedTicket); // Verify that the ticket is deleted
            Assert.AreEqual(0, await _context.Tickets.CountAsync()); // Ensure no tickets remain
        }
    }
}
