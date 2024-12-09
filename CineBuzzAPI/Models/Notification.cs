public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
    public string Type { get; set; } // e.g., "Purchase", "Review", "Like"
    public DateTime DateCreated { get; set; }
}
