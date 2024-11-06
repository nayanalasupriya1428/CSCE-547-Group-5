// @author Scott Do (Reshlynt)
// Unit tests for CartController class in MovieReviewApi.
// @date 2024-11-5
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Controllers;
using MovieReviewApi.Models;

namespace MovieReviewApi.Tests
{
    [TestClass]
    public class TicketControllerTests
    {
        private MovieContext _context;
        private TicketController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Set up the In-Memory Database for MovieContext
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString())
                .Options;

            _context = new MovieContext(options);
            SeedTestData(_context);
            _controller = new TicketController(_context);
        }

        /// <summary>
        /// Seeds the in-memory database with initial data for testing purposes.
        /// </summary>
        /// <param name="context">The in-memory movie context to be seeded with test data.</param>
        private void SeedTestData(MovieContext context)
        {
            var movie1 = new Movie { Id = 1, MovieTitle = "Movie 1", Genre = "Action", Rating = 4 };
            var movie2 = new Movie { Id = 2, MovieTitle = "Movie 2", Genre = "Comedy", Rating = 3 };

            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieId = 1, EventName = "Movie 1 Screening", Price = 15.00M, Movie = movie1 },
                new Ticket { TicketId = 2, MovieId = 1, EventName = "Movie 1 Evening Show", Price = 20.00M, Movie = movie1 },
                new Ticket { TicketId = 3, MovieId = 2, EventName = "Movie 2 Matinee", Price = 10.00M, Movie = movie2 }
            };

            context.Movie.AddRange(movie1, movie2);
            context.Tickets.AddRange(tickets);
            context.SaveChanges();
        }

        /// <summary>
        /// Tests that retrieving all tickets returns the correct list of tickets.
        /// </summary>
        [TestMethod]
        public async Task GetTickets_ShouldReturnAllTickets()
        {
            // Act
            var result = await _controller.GetTickets();

            // Assert
            Assert.IsNotNull(result);

            var tickets = result.Value as List<Ticket>;
            Assert.IsNotNull(tickets);
            Assert.AreEqual(3, tickets.Count);

            // Optionally, verify details of the tickets
            Assert.AreEqual(1, tickets[0].TicketId);
            Assert.AreEqual("Movie 1 Screening", tickets[0].EventName);
            Assert.AreEqual(15.00M, tickets[0].Price);
        }

        /// <summary>
        /// Tests that retrieving tickets by movie ID returns the correct tickets for the given movie.
        /// </summary>
        [TestMethod]
        public async Task GetTicketsByMovie_ShouldReturnTicketsForGivenMovie()
        {
            // Arrange
            int existingMovieId = 1;

            // Act
            var result = await _controller.GetTicketsByMovie(existingMovieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var tickets = okResult.Value as IEnumerable<Ticket>;
            Assert.IsNotNull(tickets);
            Assert.AreEqual(2, tickets.Count());

            var ticketList = tickets.ToList();
            Assert.AreEqual(1, ticketList[0].TicketId);
            Assert.AreEqual("Movie 1 Screening", ticketList[0].EventName);
            Assert.AreEqual(15.00M, ticketList[0].Price);
        }

        /// <summary>
        /// Tests that attempting to retrieve tickets by a non-existent movie ID returns a NotFound result.
        /// </summary>
        [TestMethod]
        public async Task GetTicketsByMovie_ShouldReturnNotFound_WhenNoTicketsExistForGivenMovie()
        {
            // Arrange
            int nonExistentMovieId = 999;

            // Act
            var result = await _controller.GetTicketsByMovie(nonExistentMovieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual($"No tickets available for movie with ID {nonExistentMovieId}.", notFoundResult.Value);
        }
    }
}
