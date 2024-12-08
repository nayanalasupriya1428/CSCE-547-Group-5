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

        /// <summary>
        /// Sets up an in-memory database and initializes the TicketService for testing.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CineBuzzDbContext(options);
            _service = new TicketService(_context);
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
        /// Tests if GetAllTicketsAsync returns all tickets when they exist.
        /// </summary>
        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsAllTickets_WhenTicketsExist()
        {
            _context.RemoveRange(_context.Tickets);
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 },
                new Ticket { TicketId = 2, MovieTimeId = 1, Price = 15.0, Quantity = 30, Availability = true, SeatNumber = 102 }
            };
            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllTicketsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Tests if GetTicketByIdAsync returns the correct ticket when it exists.
        /// </summary>
        [TestMethod]
        public async Task GetTicketByIdAsync_ReturnsTicket_WhenTicketExists()
        {
            _context.RemoveRange(_context.Tickets);
            var ticket = new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var result = await _service.GetTicketByIdAsync(ticket.TicketId);

            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.TicketId, result.TicketId);
            Assert.AreEqual(ticket.Price, result.Price);
        }

        /// <summary>
        /// Tests if AddTicketAsync adds a valid ticket successfully.
        /// </summary>
        [TestMethod]
        public async Task AddTicketAsync_AddsTicketSuccessfully_WhenTicketIsValid()
        {
            _context.RemoveRange(_context.Tickets);
            var newTicket = new Ticket { MovieTimeId = 1, Price = 12.0, Quantity = 25, Availability = true, SeatNumber = 103 };

            var result = await _service.AddTicketAsync(newTicket);

            Assert.IsNotNull(result);
            Assert.AreEqual(newTicket.Price, result.Price);
            Assert.AreEqual(1, await _context.Tickets.CountAsync());
        }

        /// <summary>
        /// Tests if UpdateTicketAsync updates the ticket details successfully when it exists.
        /// </summary>
        [TestMethod]
        public async Task UpdateTicketAsync_UpdatesTicketSuccessfully_WhenTicketExists()
        {
            _context.RemoveRange(_context.Tickets);
            var existingTicket = new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 };
            _context.Tickets.Add(existingTicket);
            await _context.SaveChangesAsync();

            var updatedTicket = new Ticket { Price = 15.0, Quantity = 40, Availability = false, SeatNumber = 101, MovieTimeId = 1 };

            var result = await _service.UpdateTicketAsync(existingTicket.TicketId, updatedTicket);

            Assert.IsNotNull(result);
            Assert.AreEqual(15.0, result.Price);
            Assert.AreEqual(40, result.Quantity);
            Assert.IsFalse(result.Availability);
        }

        /// <summary>
        /// Tests if DeleteTicketAsync removes the ticket successfully when it exists.
        /// </summary>
        [TestMethod]
        public async Task DeleteTicketAsync_DeletesTicketSuccessfully_WhenTicketExists()
        {
            _context.RemoveRange(_context.Tickets);
            var ticketToDelete = new Ticket { TicketId = 1, MovieTimeId = 1, Price = 10.0, Quantity = 50, Availability = true, SeatNumber = 101 };
            _context.Tickets.Add(ticketToDelete);
            await _context.SaveChangesAsync();

            await _service.DeleteTicketAsync(ticketToDelete.TicketId);

            var deletedTicket = await _context.Tickets.FindAsync(ticketToDelete.TicketId);
            Assert.IsNull(deletedTicket);
            Assert.AreEqual(0, await _context.Tickets.CountAsync());
        }
    }
}
