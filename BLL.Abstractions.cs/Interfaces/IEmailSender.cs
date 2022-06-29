using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes to send emails with smtp
    /// </summary>
    public interface IEmailSender
    {
        bool SendSmtpMail(EmailTemplate emailTemplate);
    }
}
