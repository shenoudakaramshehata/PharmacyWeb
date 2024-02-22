﻿
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Email
{

    public class EmailSender : IEmailSender
    {
        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "internationalhorsecarecenter@gmail.com";
            string fromPassword = "cpqswigitarlpleq";
            //string fromMail = "hallbooking3@gmail.com";
            //string fromPassword = "ssnwevikxhzcqfgh";
            //string fromMail = "slimassetssystem@gmail.com";
            //string fromPassword = "bknnhojgyjazmlzl";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;
            message.To.Add(email);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            
                smtpClient.Send(message);

           
            
        }

        public Task SendEmailAsync(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}
