using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                        MovieId = 1,
                        MovieDateTime = new DateTime(2024, 11, 6, 19, 0, 0), // 7 PM show
                        Location = "Theater 1"
                    },
                    new MovieTime
                    {
                        MovieTimeId = 2,
                        MovieId = 2,
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
                        MovieTimeId = 1,
                        Price = 10.0,
                        Quantity = 2,
                        Availability = true,
                        SeatNumber = 101
                    },
                    new Ticket
                    {
                        TicketId = 2,
                        MovieTimeId = 1,
                        Price = 12.0,
                        Quantity = 1,
                        Availability = true,
                        SeatNumber = 102
                    },
                    new Ticket
                    {
                        TicketId = 3,
                        MovieTimeId = 2,
                        Price = 15.0,
                        Quantity = 1,
                        Availability = false,
                        SeatNumber = 201
                    }
                );

                this.SaveChanges();
            }

            // Seed mock data for carts and cart items if none exist
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
                    Quantity = 2
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

           

            modelBuilder.Entity<Review>().HasData(
                new Review { ReviewId = 1, MovieId = 1, UserId = 1, Content = "Amazing movie!" },
                new Review { ReviewId = 2, MovieId = 2, UserId = 2, Content = "A classic!" }
            );

            modelBuilder.Entity<Review>()
                .HasOne<Movie>() // Each review is linked to a movie
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<Review>()
                .HasOne<User>()
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.MovieTime)
                .WithMany(mt => mt.Tickets)
                .HasForeignKey(t => t.MovieTimeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Ticket)
                .WithMany()
                .HasForeignKey(ci => ci.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentRequest>()
                .HasOne<Cart>()
                .WithOne()
                .HasForeignKey<PaymentRequest>(pr => pr.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MovieTime> MovieTimes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
       
    }
}
