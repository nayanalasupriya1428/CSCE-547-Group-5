using CineBuzzApi.Models;
namespace CineBuzzApi.Services
{
    public interface IEmailNotificationService
    {
        Task SendEmailAsync(User user, string subject, string body, NotificationType notificationType);
    }
}
