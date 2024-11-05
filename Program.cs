using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieReviewApi.Models; // Ensure this namespace is correct and points to your models

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movie Review API",
        Version = "v1",
        Description = "An API to manage movie reviews, tickets, and carts",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Email = "contact@example.com",
            Url = new Uri("https://example.com"),
        }
    });
});

// Configure in-memory database
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseInMemoryDatabase("MovieDatabase"));

var app = builder.Build();

// Seed the database
SeedDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Review API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

        if (!context.Movie.Any())
        {
            var movie1 = new Movie { Id = 1, MovieTitle = "The Shawshank Redemption", Genre = "Thriller", Rating = 5 };
            var movie2 = new Movie { Id = 2, MovieTitle = "The Godfather", Genre = "Action", Rating = 5 };

            var ticket1 = new Ticket { TicketId = 1, MovieId = movie1.Id, EventName = "Shawshank Screening", Price = 10 };
            var ticket2 = new Ticket { TicketId = 2, MovieId = movie2.Id, EventName = "Godfather Screening", Price = 12 };

            context.Movie.AddRange(movie1, movie2);
            context.Tickets.AddRange(ticket1, ticket2);
            // Seed Cart
            var cart = new Cart
            {
                CartId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemId = 1, TicketId = ticket1.TicketId, Quantity = 2, Ticket = ticket1 },
                    new CartItem { CartItemId = 2, TicketId = ticket2.TicketId, Quantity = 1, Ticket = ticket2 }
                }
               
            };

            context.Carts.Add(cart);

            context.SaveChanges();
            context.SaveChanges();
        }
    }
}

