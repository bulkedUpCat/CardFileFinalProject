using BLL.Abstractions.cs.Interfaces;
using BLL.Validation;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        private static MailMessage GetMailMessage(EmailTemplate emailTemplate)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.Body = emailTemplate.Body;
            mail.Subject = emailTemplate.Subject;
            mail.From = new MailAddress("cardFileWeb@outlook.com");
            mail.To.Add(emailTemplate.To);

            if (emailTemplate.Attachment != null)
            {
                mail.Attachments.Add(emailTemplate.Attachment);
            }

            return mail;
        }

        public bool SendSmtpMail(EmailTemplate emailTemplate)
        {
            try
            {
                MailMessage message = GetMailMessage(emailTemplate);
                //var i = _config.GetSection("Smtp")["CredentialsUsername"];
                var credentials = new NetworkCredential()
                {
                    UserName = _config.GetSection("Smtp")["CredentialsUsername"],
                    Password = _config.GetSection("Smtp")["CredentialsPassword"]
                };

                var client = new SmtpClient()
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = credentials,
                    Host = _config.GetSection("Smtp")["Host"],
                    EnableSsl = true
                };


                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                throw new CardFileException("Failed to send an email");
            }
        }
    }
}
