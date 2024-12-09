using CineBuzzApi.Data;
using CineBuzzApi.Models;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CineBuzzApi.Services
{
public class NotificationService : INotificationService
{
    private readonly IConfiguration _configuration;

    public NotificationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("info@cinebuzz.com", "CineBuzz"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(email));
        var response = await client.SendEmailAsync(msg);
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        TwilioClient.Init(accountSid, authToken);

        var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
        {
            From = new PhoneNumber(_configuration["Twilio:FromNumber"]),
            Body = message
        };
        var messageResult = await MessageResource.CreateAsync(messageOptions);
    }
}
}