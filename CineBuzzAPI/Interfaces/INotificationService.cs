using CineBuzzApi.Models;
using System.Threading.Tasks;

namespace CineBuzzApi.Services
{
    public interface INotificationService
{
    Task SendEmailAsync(string email, string subject, string message);
    
}

}