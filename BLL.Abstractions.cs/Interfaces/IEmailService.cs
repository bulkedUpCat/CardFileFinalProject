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
        Task SendTextMaterialAsPdf(User user, TextMaterial textMaterial, EmailParameters emailParams);
    }
}
