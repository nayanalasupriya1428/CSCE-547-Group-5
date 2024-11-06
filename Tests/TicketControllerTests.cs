using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            // 1. Set up the In-Memory Database for MovieContext
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString()) // Unique database for each test
                .Options;

            _context = new MovieContext(options);

            // 2. Seed the Database with Initial Data
            SeedTestData(_context);

            // 3. Set up the TicketController with the MovieContext
            _controller = new TicketController(_context);
        }

        // Helper method to seed initial data into the in-memory database
        private void SeedTestData(MovieContext context)
        {
            // Create movie data
            var movie1 = new Movie { Id = 1, MovieTitle = "Movie 1", Genre = "Action", Rating = 4 };
            var movie2 = new Movie { Id = 2, MovieTitle = "Movie 2", Genre = "Comedy", Rating = 3 };

            // Create ticket data linked to movies
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, MovieId = 1, EventName = "Movie 1 Screening", Price = 15.00M, Movie = movie1 },
                new Ticket { TicketId = 2, MovieId = 1, EventName = "Movie 1 Evening Show", Price = 20.00M, Movie = movie1 },
                new Ticket { TicketId = 3, MovieId = 2, EventName = "Movie 2 Matinee", Price = 10.00M, Movie = movie2 }
            };

            // Add movies and tickets to the context
            context.Movie.AddRange(movie1, movie2);
            context.Tickets.AddRange(tickets);
            context.SaveChanges();
        }

        [TestMethod]
        public async Task GetTickets_ShouldReturnAllTickets()
        {
            // Act
            var result = await _controller.GetTickets();

            // Assert
            // Ensure the result is not null
            Assert.IsNotNull(result);

            // Verify that the result is of type OkObjectResult
            var tickets = result.Value as List<Ticket>;
            Assert.IsNotNull(tickets);      // Ensure the tickets list is not null
            Assert.AreEqual(3, tickets.Count);  // Verify that 3 tickets were returned

            // Verify details of tickets
            Assert.AreEqual(1, tickets[0].TicketId);
            Assert.AreEqual("Movie 1 Screening", tickets[0].EventName);
            Assert.AreEqual(15.00M, tickets[0].Price);
        }

        [TestMethod]
        public async Task GetTicketsByMovie_ShouldReturnTicketsForGivenMovie()
        {
            // Arrange
            int existingMovieId = 1; // This movie ID exists in the seed data

            // Act
            var result = await _controller.GetTicketsByMovie(existingMovieId);

            // Assert
            // Ensure the result is not null
            Assert.IsNotNull(result);

            // Verify that the result is of type OkObjectResult
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            // Cast the result to OkObjectResult
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult); // Ensure the cast was successful

            // Access the list of tickets from the result
            var tickets = okResult.Value as IEnumerable<Ticket>;
            Assert.IsNotNull(tickets); // Ensure the returned value is not null

            // Verify that the correct number of tickets are returned (2 tickets linked to movie ID 1)
            Assert.AreEqual(2, tickets.Count());

            // Optionally verify the details of the tickets
            var ticketList = tickets.ToList();
            Assert.AreEqual(1, ticketList[0].TicketId);
            Assert.AreEqual("Movie 1 Screening", ticketList[0].EventName);
            Assert.AreEqual(15.00M, ticketList[0].Price);
        }

        [TestMethod]
        public async Task GetTicketsByMovie_ShouldReturnNotFound_WhenNoTicketsExistForGivenMovie()
        {
            // Arrange
            int nonExistentMovieId = 999; // This movie ID does not exist in the seed data

            // Act
            var result = await _controller.GetTicketsByMovie(nonExistentMovieId);

            // Assert
            // Ensure the result is not null
            Assert.IsNotNull(result);

            // Verify that the result is of type NotFoundResult
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));

            // Cast the result to NotFoundObjectResult to check the message
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual($"No tickets available for movie with ID {nonExistentMovieId}.", notFoundResult.Value);
        }


    }
}
