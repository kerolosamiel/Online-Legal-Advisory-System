using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;

namespace ELawyer.Utility
{
    //Note:
    //that to solve the Exception of (Can not resolve IemailSender)
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }
        //Note:
        //Retrive the secret value and assign it into the above property
        public EmailSender(IConfiguration _config)
        {
            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }



        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //logic to send Email

            var client = new SendGridClient(SendGridSecret);

            var from = new EmailAddress("mustafa.saad.sw@gmail.com", " ELawyer");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(message);


        }
    }

}

