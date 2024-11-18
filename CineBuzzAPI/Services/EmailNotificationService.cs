using CineBuzzApi.Models;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CineBuzzApi.Models;

namespace CineBuzzApi.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly SmtpClient _smtpClient;

        public EmailNotificationService()
        {
            _smtpClient = new SmtpClient("smtp.your-email-provider.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@example.com", "your-password"),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(User user, string subject, string body, NotificationType notificationType)
        {
            if (user.NotificationPreference == null || !user.NotificationPreference.ReceiveEmailNotifications)
            {
                return; // Skip if user opted out of email notifications
            }

            // Check if the notification type matches user's preferences
            if (user.NotificationPreference.PreferredNotificationTypes != null &&
                !user.NotificationPreference.PreferredNotificationTypes.Contains(notificationType))
            {
                return; // Skip if the notification type is not preferred
            }

            var mailMessage = new MailMessage("your-email@example.com", user.Email, subject, body)
            {
                IsBodyHtml = true // Set to true if the body contains HTML
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
