using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ELawyer.Utility;

//Note:
//that to solve the Exception of (Can not resolve IemailSender)
public class EmailSender : IEmailSender
{
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly string _sendGridKey;

    //Note:
    //Retrive the secret value and assign it into the above property
    public EmailSender(IConfiguration configuration)
    {
        _sendGridKey = configuration["SendGrid:SendGridSecret"] ?? "";
        _fromEmail = configuration["SendGrid:FromEmail"] ?? "";
        _fromName = configuration["SendGrid:FromName"] ?? "";

        if (string.IsNullOrEmpty(_sendGridKey)) throw new ArgumentNullException("SendGrid API Key is not configured");
    }


    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //logic to send Email
        var client = new SendGridClient(_sendGridKey);

        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(email);
        var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

        return client.SendEmailAsync(message);
    }
}