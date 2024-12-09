using System.Text.Json.Serialization;

namespace CineBuzzApi.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int MovieTimeId { get; set; }
        public required double Price { get; set; }
        public int Quantity { get; set; }
        public required bool Availability { get; set; }
        public int SeatNumber { get; set; }

        // Make MovieTime optional and ignored in JSON serialization
        [JsonIgnore]
        public MovieTime? MovieTime { get; set; }
    }
    public class EditTicketRequest
{
    public double? Price { get; set; }
    public int? Quantity { get; set; }
}
}
