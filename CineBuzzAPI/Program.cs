using Microsoft.EntityFrameworkCore;
using CineBuzzApi.Services;
using CineBuzzApi.Models;
using CineBuzzApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Register services

builder.Services.AddScoped<IRecentlyViewedMoviesService, RecentlyViewedMoviesService>(); // Register with interface
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPaymentRequestService, PaymentRequestService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddDbContext<CineBuzzDbContext>(options =>
    options.UseInMemoryDatabase("CineBuzzDb"));  // Using in-memory database

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Logging API keys to verify configuration is loaded correctly (only for debugging purposes, remove in production)
var sendGridApiKey = builder.Configuration["SendGrid:ApiKey"];
var twilioAccountSid = builder.Configuration["Twilio:AccountSid"];
Console.WriteLine($"SendGrid API Key: {sendGridApiKey}");
Console.WriteLine($"Twilio Account SID: {twilioAccountSid}");

// Enable Swagger for development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
