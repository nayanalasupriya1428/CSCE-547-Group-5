using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineBuzzApi.Data
{
    public class CineBuzzDbContext : DbContext
    {
        public CineBuzzDbContext(DbContextOptions<CineBuzzDbContext> options)
            : base(options)
        {
            // Seed mock data for users if none exist
            if (Users != null && !Users.Any())
            {
                Users.AddRange(
                    new User { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" },
                    new User { Id = 2, Email = "jane.doe@example.com", Username = "Janedoe", FirstName = "Jane", LastName = "Doe", Password = "23456788" }
                );

                this.SaveChanges();
            }

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

            // Seed mock data for reviews if none exist
            if (Reviews != null && !Reviews.Any())
            {
                Reviews.AddRange(
                    new Review { ReviewId = 1, UserId = 1, MovieId = 1, Content = "Amazing movie with great visuals!" },
                    new Review { ReviewId = 2, UserId = 2, MovieId = 2, Content = "Thought-provoking and revolutionary!" }
                );

                this.SaveChanges();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new { Id = 1, Email = "john.doe@example.com", Username = "Johndoe", FirstName = "John", LastName = "Doe", Password = "12345678" },
                new { Id = 2, Email = "jane.doe@example.com", Username = "Janedoe", FirstName = "Jane", LastName = "Doe", Password = "23456788" }
            );

            // **Users Table**
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id); // Primary key

                // Email field
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(255) // Limit email length to 255 characters
                    .HasAnnotation("EmailPattern", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"); // Enforce email format at database level (if supported)

                entity.HasIndex(u => u.Email)
                    .IsUnique(); // Enforce unique email constraint

                // Username field
                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(u => u.Username)
                    .IsUnique(); // Enforce unique username constraint for account uniqueness

                // Password field
                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(128); // Account for hashed passwords

                // FirstName and LastName fields
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(50); // Ensure a reasonable limit on names

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            // **Movies Table**
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId); // Primary key

                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(m => m.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                // Define Genres as a string if stored in a single column
                entity.Property(m => m.Genres)
                    .HasConversion(
                        genres => string.Join(',', genres), // Convert list to string for storage
                        genres => genres.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .IsRequired();
            });


            // **MovieTimes Table**
            modelBuilder.Entity<MovieTime>(entity =>
            {
                entity.HasKey(mt => mt.MovieTimeId); // Primary key

                entity.Property(mt => mt.MovieDateTime)
                    .IsRequired();

                entity.Property(mt => mt.Location)
                    .IsRequired()
                    .HasMaxLength(255);

                // Define foreign key relationship to Movie
                entity.HasOne<Movie>() // Define relationship without navigation property
                    .WithMany()        // Movie doesn't have a MovieTimes collection
                    .HasForeignKey(mt => mt.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Add index for query performance
                entity.HasIndex(mt => mt.MovieDateTime);

                // Unique constraint to avoid duplicate MovieTime entries
                entity.HasIndex(mt => new { mt.MovieId, mt.MovieDateTime, mt.Location })
                    .IsUnique();
            });


            // **Tickets Table**
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.TicketId); // PK

                // Configure Price field
                entity.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(10, 2); // Specify precision for monetary values


                // Configure Quantity field
                entity.Property(t => t.Quantity)
                    .IsRequired();

                // Configure SeatNumber field
                entity.Property(t => t.SeatNumber)
                    .IsRequired();

                // Foreign key relationship with MovieTime
                entity.HasOne<MovieTime>() // Reference MovieTime without requiring a navigation property on MovieTime
                    .WithMany(mt => mt.Tickets)
                    .HasForeignKey(t => t.MovieTimeId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete when a MovieTime is removed

                // Unique constraint to prevent duplicate tickets for the same seat and showtime
                entity.HasIndex(t => new { t.MovieTimeId, t.SeatNumber })
                    .IsUnique();
            });


            // **Carts Table**
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId); // Primary key

                // Configure Total field as decimal for monetary precision
                entity.Property(c => c.Total)
                    .IsRequired()
                    .HasPrecision(10, 2); // Set precision for monetary values

                // Foreign key relationship with User
                entity.HasOne(c => c.User)
                    .WithMany() // User does not have a navigation property for carts
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete carts when a user is deleted
            });


            // **CartItems Table**
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.CartItemId); // Primary key

                // Configure Quantity field
                entity.Property(ci => ci.Quantity)
                    .IsRequired();

                // Foreign key relationship with Cart
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.Items)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete CartItems when Cart is deleted

                // Foreign key relationship with Ticket
                entity.HasOne(ci => ci.Ticket)
                    .WithMany() // Ticket does not have a navigation property for CartItems
                    .HasForeignKey(ci => ci.TicketId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Tickets if they are in a Cart

                // Add unique constraint to prevent duplicate CartItems for the same Ticket in the same Cart
                entity.HasIndex(ci => new { ci.CartId, ci.TicketId })
                    .IsUnique();
            });


            // **PaymentRequests Table**
            modelBuilder.Entity<PaymentRequest>(entity =>
            {
                entity.HasKey(pr => pr.PaymentRequestId); // Primary key

                // Configure CardNumber field
                entity.Property(pr => pr.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16); // Basic length validation for card numbers

                // Configure ExpirationDate field
                entity.Property(pr => pr.ExpirationDate)
                    .IsRequired()
                    .HasMaxLength(5); // Format: MM/YY

                // Configure CardholderName field
                entity.Property(pr => pr.CardholderName)
                    .IsRequired()
                    .HasMaxLength(255); // Allow up to 255 characters for cardholder name

                // Configure CVC field
                entity.Property(pr => pr.CVC)
                    .IsRequired()
                    .HasMaxLength(4); // Support both 3- and 4-digit CVCs

                // Foreign key relationship with Cart
                entity.HasOne<Cart>() // Define FK relationship with Cart
                    .WithOne() // One-to-one relationship
                    .HasForeignKey<PaymentRequest>(pr => pr.CartId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete PaymentRequest when Cart is deleted
            });


            // **Reviews Table**
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.ReviewId); // Primary key

                // Configure Content field
                entity.Property(r => r.Content)
                    .IsRequired()
                    .HasMaxLength(2000); // Limit review length to 2000 characters

                // Foreign key relationship with Movie
                entity.HasOne<Movie>() // Define FK relationship with Movie
                    .WithMany()        // Movie does not have navigation property for Reviews
                    .HasForeignKey(r => r.MovieId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete reviews when a Movie is deleted

                // Foreign key relationship with User
                entity.HasOne<User>() // Define FK relationship with User
                    .WithMany()       // User does not have navigation property for Reviews
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete reviews when a User is deleted

                // Add index to improve query performance for reviews by User or Movie
                entity.HasIndex(r => new { r.UserId, r.MovieId });
            });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MovieTime> MovieTimes { get; set; } // Add DbSet for MovieTime
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // dotnet ef migrations add ___________
        // dotnet ef database update
    }
}
