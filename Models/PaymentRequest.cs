namespace MovieReviewApi.Models
{
public class PaymentRequest
{
    public int CartId { get; set; }
    public string CardNumber { get; set; } // Ideally, you wouldn't store this in a real-world app
    public string ExpirationDate { get; set; } // Format: MM/yyyy
    public string CardholderName { get; set; }
    public string CVC { get; set; } // Ideally, you wouldn't store this in a real-world app
}
}
