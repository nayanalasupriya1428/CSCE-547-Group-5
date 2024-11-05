using System.Text.Json.Serialization;
namespace MovieReviewApi.Models
{
public class Ticket
{
    public int TicketId { get; set; }
    public int MovieId { get; set; }
    public string EventName { get; set; } 
    public decimal Price { get; set; }
    
    // Navigation property
    [JsonIgnore]
    public Movie Movie { get; set; }
}
}
