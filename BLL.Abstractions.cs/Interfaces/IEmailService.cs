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
    public interface IEmailService
    {
        void SendTextMaterialAsPdf(User user, TextMaterial textMaterial, EmailParameters emailParams);
        void NotifyThatTextMaterialWasCreated(User user, TextMaterial textMaterial);
        void NotifyThatTextMaterialWasDeleted(User user, TextMaterial textMaterial);
        void NotifyThatTextMaterialWasApproved(User user, TextMaterial textMaterial);
        void NotifyThatTextMaterialWasRejected(User user, TextMaterial textMaterial, string? rejectMessage = null);
    }
}
