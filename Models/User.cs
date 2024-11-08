namespace MovieReviewApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public UserPreference NotificationPreference { get; set; }
    }

    public enum UserPreference
    {
        SMS,
        Email,
        Both,
        None
    }
}
