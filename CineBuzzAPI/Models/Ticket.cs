using System.Text.Json.Serialization;

namespace CineBuzzApi.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int MovieTimeId { get; set; }
        public int MovieId { get; set; }
        public double Price { get; set; }  // Assuming non-nullable, will require validation elsewhere if needed
        public int Quantity { get; set; }
        public bool Availability { get; set; }  // Non-nullable boolean
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

