using Core.Models;
using Core.RequestFeatures;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes to work with email notifications
    /// </summary>
    public interface IEmailService
    {
        void SendTextMaterialAsPdf(User user, TextMaterial textMaterial, EmailParameters emailParams);
        void NotifyThatTextMaterialWasCreated(User user, TextMaterial textMaterial);
        void NotifyThatTextMaterialWasDeleted(User user, TextMaterial textMaterial);
        void NotifyThatTextMaterialWasApproved(User user, TextMaterial textMaterial);
        void NotifyThatTextMaterialWasRejected(User user, TextMaterial textMaterial, string? rejectMessage = null);
        void SendListOfTextMaterialsOfTheUser(User user, string email);
    }
}
