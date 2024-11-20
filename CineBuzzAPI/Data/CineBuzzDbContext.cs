using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineBuzzApi.Data
{
    public class CineBuzzDbContext : DbContext
    {
        public CineBuzzDbContext(DbContextOptions<CineBuzzDbContext> options)
            : base(options)
        {
            // Seed mock data for movies if none exist
            if (Movies != null && !Movies.Any())
            {
                Movies.AddRange(
                    new Movie
                    {
                        MovieId = 1,
                        Title = "Inception",
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        Genres = new List<string> { "Action", "Sci-Fi", "Thriller" }
                    },
                    new Movie
                    {
                        MovieId = 2,
                        Title = "The Matrix",
                        Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                        Genres = new List<string> { "Action", "Sci-Fi" }
                    }
                );

                this.SaveChanges();
            }


            // Seed mock data for MovieTime if none exist
            if (MovieTimes != null && !MovieTimes.Any())
            {
                MovieTimes.AddRange(
                    new MovieTime
                    {
                        MovieTimeId = 1,
                        MovieId = 1,  // Assuming Movie with ID 1 exists
                        MovieDateTime = new DateTime(2024, 11, 6, 19, 0, 0), // 7 PM show
                        Location = "Theater 1"
                    },
                    new MovieTime
                    {
                        MovieTimeId = 2,
                        MovieId = 2,  // Assuming Movie with ID 2 exists
                        MovieDateTime = new DateTime(2024, 11, 6, 21, 0, 0), // 9 PM show
                        Location = "Theater 2"
                    }
                );

                this.SaveChanges();
            }

            // Seed mock data for tickets if none exist
            if (Tickets != null && !Tickets.Any())
            {
                Tickets.AddRange(
                    new Ticket
                    {
                        TicketId = 1,
                        MovieTimeId = 1,    // Associated with MovieTime ID 1
                        Price = 10.0,
                        Quantity = 2,
                        Availability = true,
                        SeatNumber = 101
                    },
                    new Ticket
                    {
                        TicketId = 2,
                        MovieTimeId = 1,    // Associated with MovieTime ID 1
                        Price = 12.0,
                        Quantity = 1,
                        Availability = true,
                        SeatNumber = 102
                    },
                    new Ticket
                    {
                        TicketId = 3,
                        MovieTimeId = 2,    // Associated with MovieTime ID 2
                        Price = 15.0,
                        Quantity = 1,
                        Availability = false,
                        SeatNumber = 201
                    }
                );

                this.SaveChanges();

            }




            if (PaymentRequests != null && !PaymentRequests.Any())
            {
                PaymentRequests.AddRange(
                    new PaymentRequest
                    {
                        PaymentRequestId = 1,
                        CartId = 1,
                        CardNumber = "4111111111111111",
                        ExpirationDate = "12/25",
                        CardholderName = "John Doe",
                        CVC = "123"
                    },
                    new PaymentRequest
                    {
                        PaymentRequestId = 2,
                        CartId = 2,
                        CardNumber = "5555555555554444",
                        ExpirationDate = "11/24",
                        CardholderName = "Jane Smith",
                        CVC = "456"
                    }
                );

                this.SaveChanges();
            }


            // Add mock data for carts and cart items
            if (Carts != null && !Carts.Any())
            {
                var cart1 = new Cart
                {
                    CartId = 1,
                    UserId = 1,
                    Total = 100.0
                };

                var cartItem1 = new CartItem
                {
                    CartItemId = 1,
                    CartId = 1,
                    TicketId = 1,
                    Quantity = 2,
                };

                cart1.Items.Add(cartItem1);
                Carts.Add(cart1);
                this.SaveChanges();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" },
                new { Id = 2, Email = "jane.doe@example.com", Username = "Janedoe", FirstName = "Jane", LastName = "Doe", Password = "23456788" }
            );

            // Assume NotificationPreferencesId is a foreign key in User for linking
            modelBuilder.Entity<NotificationPreferences>().HasData(
                new { Id = 1, UserId = 1, ReceiveEmailNotifications = true, Frequency = NotificationFrequency.Daily },
                new { Id = 2, UserId = 2, ReceiveEmailNotifications = true, Frequency = NotificationFrequency.Weekly }
            );

            // If using NotificationTypes as a linked entity, you'll need to seed this data separately and handle it appropriately.

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MovieTime> MovieTimes { get; set; } // Add DbSet for MovieTime
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<NotificationPreferences> NotificationPreferences { get; set; }
        public DbSet<Review> Reviews { get; set; }




    }
}
