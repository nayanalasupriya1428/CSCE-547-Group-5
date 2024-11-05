using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Models;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}
