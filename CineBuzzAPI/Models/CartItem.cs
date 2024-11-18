using System.Text.Json.Serialization;

namespace CineBuzzApi.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public Cart Cart { get; set; } // Ignore this property to prevent circular reference

        public Ticket Ticket { get; set; }
    }
}
