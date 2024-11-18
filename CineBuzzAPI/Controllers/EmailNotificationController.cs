using CineBuzzApi.Models;
using CineBuzzApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CineBuzzApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailNotificationController : ControllerBase
    {
        private readonly IEmailNotificationService _emailService;
        private readonly IUserService _userService;

        public EmailNotificationController(IEmailNotificationService emailService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            var user = await _userService.GetUserByIdAsync(emailRequest.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _emailService.SendEmailAsync(user, emailRequest.Subject, emailRequest.Body, emailRequest.NotificationType);
            return Ok("Email sent based on preferences.");
        }
    }

    public class EmailRequest
    {
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
