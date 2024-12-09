using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Data;
using CineBuzzApi.Models;
using CineBuzzApi.Services;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    [TestClass]
    public class TicketServiceTests
    {
        private CineBuzzDbContext _context;
        private TicketService _ticketService;

        [TestInitialize]
        public void Setup()
        {
            // Configure an in-memory database for testing
            var options = new DbContextOptionsBuilder<CineBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CineBuzzDbContext(options);
            _ticketService = new TicketService(_context);

            // Seed the database with initial data for tests
            SeedDatabase();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            // Add a movie to the database
            var movie = new Movie
            {
                MovieId = 201,
                Title = "Inception",
                Description = "A mind-bending thriller about dreams within dreams.",
                Genres = new List<string> { "Action", "Sci-Fi", "Thriller" }
            };


            // Add some initial MovieTimes and Tickets to the database with unique IDs
            var movieTime1 = new MovieTime { MovieTimeId = 101, MovieId = 201, MovieDateTime = new System.DateTime(2024, 12, 9, 19, 0, 0), Location = "Theater 1" };
            var movieTime2 = new MovieTime { MovieTimeId = 102, MovieId = 201, MovieDateTime = new System.DateTime(2024, 12, 9, 21, 0, 0), Location = "Theater 2" };

            _context.MovieTimes.AddRange(movieTime1, movieTime2);

            var tickets = new[]
            {
                new Ticket { TicketId = 301, MovieTimeId = 101, Price = 10.0, Quantity = 1, Availability = true, SeatNumber = 1 },
                new Ticket { TicketId = 302, MovieTimeId = 101, Price = 10.0, Quantity = 1, Availability = true, SeatNumber = 2 },
                new Ticket { TicketId = 303, MovieTimeId = 102, Price = 15.0, Quantity = 1, Availability = true, SeatNumber = 1 }
            };

            _context.Tickets.AddRange(tickets);
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsAllTickets()
        {
            // Act
            var result = await _ticketService.GetAllTicketsAsync();

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            var ticketList = result as List<Ticket>;
            Assert.AreEqual(6, ticketList.Count); // Verify the correct number of tickets is returned

            // Validate the details of the tickets
            Assert.AreEqual(301, ticketList[3].TicketId);
            Assert.AreEqual(10.0, ticketList[3].Price);
            Assert.AreEqual(1, ticketList[3].SeatNumber);
            Assert.AreEqual(101, ticketList[3].MovieTimeId);

            Assert.AreEqual(302, ticketList[4].TicketId);
            Assert.AreEqual(10.0, ticketList[4].Price);
            Assert.AreEqual(2, ticketList[4].SeatNumber);
            Assert.AreEqual(101, ticketList[4].MovieTimeId);

            Assert.AreEqual(303, ticketList[5].TicketId);
            Assert.AreEqual(15.0, ticketList[5].Price);
            Assert.AreEqual(1, ticketList[5].SeatNumber);
            Assert.AreEqual(102, ticketList[5].MovieTimeId);
        }
        [TestMethod]
        public async Task GetTicketByIdAsync_ReturnsCorrectTicket_WhenTicketExists()
        {
            // Arrange
            int ticketId = 301; // This ID matches a ticket in the seeded database

            // Act
            var result = await _ticketService.GetTicketByIdAsync(ticketId);

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual(ticketId, result.TicketId); // Verify the ticket ID matches
            Assert.AreEqual(10.0, result.Price); // Verify the ticket price
            Assert.AreEqual(1, result.SeatNumber); // Verify the seat number
            Assert.AreEqual(101, result.MovieTimeId); // Verify the MovieTimeId
        }
    }
}
