using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly string smtpHost = "smtp.example.com"; // Replace with your SMTP server
        private readonly int smtpPort = 587; // Common ports are 25, 465, 587
        private readonly string smtpUser = "your-email@example.com"; // Your email address
        private readonly string smtpPass = "your-email-password"; // Your email password

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromBody] MailRequest mailRequest)
        {
            try
            {
                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpUser),
                        Subject = mailRequest.Subject,
                        Body = mailRequest.Body,
                        IsBodyHtml = true, // Set to true if the body contains HTML
                    };

                    mailMessage.To.Add(mailRequest.ToEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return Ok(new { message = "Mail sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending mail.", error = ex.Message });
            }
        }
    }

    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
