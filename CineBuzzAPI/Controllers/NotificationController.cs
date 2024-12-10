using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("sendEmail")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        await _notificationService.SendEmailAsync(request.Email, request.Subject, request.Message);
        return Ok("Email sent successfully.");
    }

}
}

public class EmailRequest
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}


