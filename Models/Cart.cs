namespace MovieReviewApi.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        // Navigation property
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}

