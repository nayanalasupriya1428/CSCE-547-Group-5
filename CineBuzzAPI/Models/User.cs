namespace CineBuzzApi.Models
{
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public NotificationPreferences NotificationPreference { get; set; } = new NotificationPreferences();
}

public class NotificationPreferences
{
    public bool ReceiveEmailNotifications { get; set; }
    public NotificationFrequency Frequency { get; set; } // Daily, Weekly, etc.
    public List<NotificationType> PreferredNotificationTypes { get; set; } = new List<NotificationType>(); // Default empty list
}


public enum NotificationFrequency
{
    Instant,
    Daily,
    Weekly
}

public enum NotificationType
{
    Promotion,
    Update,
    Alert
}
}