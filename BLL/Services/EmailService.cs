using BLL.Abstractions.cs.Interfaces;
using BLL.Validation;
using Core.Models;
using Core.RequestFeatures;
using DAL.Abstractions.Interfaces;
using DAL.Data;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public EmailService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task SendTextMaterialAsPdf(User user, TextMaterial textMaterial, EmailParameters emailParams)
        {
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(writer);
            var pdf = CreateDocument(pdfDocument, textMaterial, emailParams);

            pdf.Close();
            MemoryStream pdfStream = new MemoryStream(stream.ToArray());

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Text material",
                    Body = "Here is your text material in pdf format",
                    Attachment = new Attachment(pdfStream, "textMaterial.pdf")
                });
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
            finally
            {
                stream.Close();
                pdfStream.Close();
                writer.Close();
            }
        }

        private Document CreateDocument(PdfDocument pdf, TextMaterial textMaterial, EmailParameters emailParams)
        {
            var document = new Document(pdf);

            if (emailParams.Title != null)
            {
                document.Add(new Paragraph($"TITLE: {textMaterial.Title}"));
            }

            if (emailParams.Category != null)
            {
                document.Add(new Paragraph());
                document.Add(new Paragraph($"CATEGORY: {textMaterial.TextMaterialCategory.Title}"));
                document.Add(new Paragraph());
            }

            foreach(var element in HtmlConverter.ConvertToElements(textMaterial.Content))
            {
                var temp = (IBlockElement)element;
                document.Add(temp);
            }

            if (emailParams.Author != null)
            {
                document.Add(new Paragraph($"AUTHOR: {textMaterial.Author.UserName}"));
            }

            if (emailParams.DatePublished != null)
            {
                document.Add(new Paragraph($"DATE PUBLISHED: {textMaterial.DatePublished}"));
            }

            return document;
        }
    }
}
