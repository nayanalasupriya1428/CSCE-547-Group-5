// @author Scott Do (Reshlynt)
// Unit tests for CartController class in MovieReviewApi.
// @date 2024-11-5

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Models;

namespace MovieReviewApi.Tests
{
    [TestClass]
    public class CartControllerTests
    {
        private MovieContext _context;
        private CartController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Set up In-Memory Database for MovieContext with unique database for each test
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString())
                .Options;

            _context = new MovieContext(options);
            SeedTestData(_context);
            _controller = new CartController(_context);
        }

        /// <summary>
        /// Seeds the in-memory database with initial data for testing purposes.
        /// </summary>
        /// <param name="context">The in-memory movie context to be seeded with test data.</param>
        private void SeedTestData(MovieContext context)
        {
            var cart = new Cart
            {
                CartId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemId = 1, CartId = 1, TicketId = 1, Quantity = 2 },
                    new CartItem { CartItemId = 2, CartId = 1, TicketId = 2, Quantity = 1 }
                }
            };

            var ticket1 = new Ticket
            {
                TicketId = 1,
                EventName = "Movie 1",
                Price = 15.00M
            };

            var ticket2 = new Ticket
            {
                TicketId = 2,
                EventName = "Movie 2",
                Price = 10.00M
            };

            context.Carts.Add(cart);
            context.Tickets.AddRange(ticket1, ticket2);
            context.SaveChanges();
        }

        /// <summary>
        /// Tests that a ticket can be added to an existing cart successfully.
        /// </summary>
        [TestMethod]
        public async Task AddTicketToCart_ShouldAddTicketSuccessfully()
        {
            // Arrange
            int cartId = 1;
            int ticketId = 1;
            int quantity = 1;

            // Act
            var result = await _controller.AddTicketToCart(cartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var messageProperty = okResult.Value.GetType().GetProperty("Message");
            Assert.IsNotNull(messageProperty, "Message property should exist in response");
            var message = messageProperty.GetValue(okResult.Value) as string;
            Assert.AreEqual("Ticket added to cart successfully.", message);

            var updatedCart = await _context.Carts.Include(c => c.CartItems)
                                                  .FirstOrDefaultAsync(c => c.CartId == cartId);
            Assert.IsNotNull(updatedCart);

            var cartItem = updatedCart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
            Assert.IsNotNull(cartItem);
            Assert.AreEqual(3, cartItem.Quantity); // Existing quantity (2) + added quantity (1) = 3
        }

        /// <summary>
        /// Tests that adding a ticket to a non-existent cart returns NotFound.
        /// </summary>
        [TestMethod]
        public async Task AddTicketToCart_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            // Arrange
            int nonExistentCartId = 999;
            int ticketId = 1;
            int quantity = 1;

            // Act
            var result = await _controller.AddTicketToCart(nonExistentCartId, ticketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Cart not found.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests that adding a non-existent ticket to a cart returns NotFound.
        /// </summary>
        [TestMethod]
        public async Task AddTicketToCart_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int cartId = 1;
            int nonExistentTicketId = 999;
            int quantity = 1;

            // Act
            var result = await _controller.AddTicketToCart(cartId, nonExistentTicketId, quantity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Ticket not found.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests that a ticket can be removed from an existing cart successfully.
        /// </summary>
        [TestMethod]
        public async Task RemoveTicketFromCart_ShouldRemoveTicketSuccessfully()
        {
            // Arrange
            int cartId = 1;
            int ticketId = 1;

            // Act
            var result = await _controller.RemoveTicketFromCart(cartId, ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as dynamic;
            Assert.IsNotNull(response);

            var updatedCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
            Assert.IsNotNull(updatedCart);

            var removedItem = updatedCart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
            Assert.IsNull(removedItem);

            var totalAmount = updatedCart.CartItems.Sum(ci => ci.Quantity * ci.Ticket.Price);
            var totalProperty = response.GetType().GetProperty("Total");
            var total = totalProperty.GetValue(response) as decimal?;
            Assert.AreEqual(totalAmount, total);
        }

        /// <summary>
        /// Tests that attempting to remove a ticket from a non-existent cart returns NotFound.
        /// </summary>
        [TestMethod]
        public async Task RemoveTicketFromCart_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            // Arrange
            int nonExistentCartId = 999;
            int ticketId = 1;

            // Act
            var result = await _controller.RemoveTicketFromCart(nonExistentCartId, ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Cart not found.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests that attempting to remove a ticket not in the cart returns NotFound.
        /// </summary>
        [TestMethod]
        public async Task RemoveTicketFromCart_ShouldReturnNotFound_WhenTicketNotInCart()
        {
            // Arrange
            int cartId = 1;
            int ticketNotInCartId = 3;

            // Act
            var result = await _controller.RemoveTicketFromCart(cartId, ticketNotInCartId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Ticket not found in cart.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests that an existing cart can be retrieved successfully.
        /// </summary>
        [TestMethod]
        public async Task GetCart_ShouldReturnExistingCart()
        {
            // Arrange
            int existingCartId = 1;

            // Act
            var result = await _controller.GetCart(existingCartId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var cart = okResult.Value as Cart;
            Assert.IsNotNull(cart);
            Assert.AreEqual(existingCartId, cart.CartId);
            Assert.AreEqual(2, cart.CartItems.Count);
        }

        /// <summary>
        /// Tests that attempting to retrieve a non-existent cart returns NotFound.
        /// </summary>
        [TestMethod]
        public async Task GetCart_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            // Arrange
            int nonExistentCartId = 999;

            // Act
            var result = await _controller.GetCart(nonExistentCartId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.AreEqual("Cart not found.", notFoundResult.Value);
        }

        /// <summary>
        /// Tests that a new cart can be created successfully when no cart ID is provided.
        /// </summary>
        [TestMethod]
        public async Task GetCart_ShouldCreateNewCart_WhenNoCartIdProvided()
        {
            // Arrange
            int initialCartCount = _context.Carts.Count();

            // Act
            var result = await _controller.GetCart();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var newCart = okResult.Value as Cart;
            Assert.IsNotNull(newCart);
            Assert.AreNotEqual(0, newCart.CartId);

            int newCartCount = _context.Carts.Count();
            Assert.AreEqual(initialCartCount + 1, newCartCount);
        }
    }
}
