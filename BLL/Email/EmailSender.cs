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
    /// <summary>
    /// Class to perform various operation regarding send email messages via smtp client
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor which takes one argument
        /// </summary>
        /// <param name="config">Instance of class that implements IConfiguration interfece to have access to appsettings.json file</param>
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates an instance of class MailMessage from the template
        /// </summary>
        /// <param name="emailTemplate">Instance of custom class EmailTemplate which contains all info for creating an instance of class MailMessage</param>
        /// <returns></returns>
        private static MailMessage GetMailMessage(EmailTemplate emailTemplate)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.Body = emailTemplate.Body;
            mail.Subject = emailTemplate.Subject;
            mail.From = new MailAddress("cardFileWeb2@outlook.com");
            mail.To.Add(emailTemplate.To);

            if (emailTemplate.Attachment != null)
            {
                mail.Attachments.Add(emailTemplate.Attachment);
            }

            return mail;
        }

        /// <summary>
        /// Sends an email using smtp client
        /// </summary>
        /// <param name="emailTemplate">Instance of custom class EmailTemplate which contains all info for creating an instance of class MailMessage</param>
        /// <returns>True if email was successfully sent, throws exception otherwise</returns>
        /// <exception cref="CardFileException"></exception>
        public bool SendSmtpMail(EmailTemplate emailTemplate)
        {
            try
            {
                MailMessage message = GetMailMessage(emailTemplate);
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
