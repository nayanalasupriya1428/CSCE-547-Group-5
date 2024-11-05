namespace MovieReviewApi.Models
{
public class CartItem
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }  // Foreign key
    public int TicketId { get; set; }  // Foreign key
    public int Quantity { get; set; }

    // Navigation properties
    public Cart Cart { get; set; }
    public Ticket Ticket { get; set; }
}
}