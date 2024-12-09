using System.Text.Json.Serialization;

namespace CineBuzzApi.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int MovieTimeId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Availability { get; set; }
        public int SeatNumber { get; set; }

        [JsonIgnore]
        public MovieTime? MovieTime { get; set; }
    }
}
