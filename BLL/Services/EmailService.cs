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
    /// <summary>
    /// Service to perform various operation regarding working with email such as sending a text material's info on the particular user's email,
    /// notifying the user of the creation, deleting, approval and rejection of their text material with a message on their email
    /// </summary>
    public class EmailService: IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Constructor which takes three arguments
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="userManager">Instance of class UserManager to perform operations on users</param>
        /// <param name="emailSender">Instance of class EmailSender to perform operations with smtp client</param>
        public EmailService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Sends the text material's info on user's email
        /// </summary>
        /// <param name="user">User to receive an email</param>
        /// <param name="textMaterial">Text material which info is to be sent</param>
        /// <param name="emailParams">Parameters that will be present in a generated pdf file</param>
        /// <exception cref="CardFileException"></exception>
        public void SendTextMaterialAsPdf(User user, TextMaterial textMaterial, EmailParameters emailParams)
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

        /// <summary>
        /// Helper method to generate a pdf file taking in account email parameters
        /// </summary>
        /// <param name="pdf">PdfDocument</param>
        /// <param name="textMaterial">Text material which info will be sent</param>
        /// <param name="emailParams">Parameters that will be present in a generated pdf file</param>
        /// <returns>Generated pdf document</returns>
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

        /// <summary>
        /// Sends a notification on user's email if their text material was created
        /// </summary>
        /// <param name="user">The author of the text material</param>
        /// <param name="textMaterial">Text material that was recently created</param>
        /// <exception cref="CardFileException"></exception>
        public void NotifyThatTextMaterialWasCreated(User user, TextMaterial textMaterial)
        {
            var body = $"Hello {user.UserName}." +
                $"You have just created a new text material with title '{textMaterial.Title}'." +
                "Currently its approval status is PENDING. We will let you know when it's approved or rejected.";

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Text material created",
                    Body = body,
                    Attachment = null
                });
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Sends a notification on user's email if their text material was deleted
        /// </summary>
        /// <param name="user">The author of text material</param>
        /// <param name="textMaterial">Text material that was recently deleted</param>
        /// <exception cref="CardFileException"></exception>
        public void NotifyThatTextMaterialWasDeleted(User user, TextMaterial textMaterial)
        {
            var body = $"Hello {user.UserName}." +
                $"Your text material '{textMaterial.Title}' was deleted.";

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Text material deleted",
                    Body = body,
                    Attachment = null
                });
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Sends a notification on user's email if their text material was approved
        /// </summary>
        /// <param name="user">The author of text material</param>
        /// <param name="textMaterial">Text material that was recently approved</param>
        /// <exception cref="CardFileException"></exception>
        public void NotifyThatTextMaterialWasApproved(User user, TextMaterial textMaterial)
        {
            var body = $"Hello {user.UserName}." +
                $"Your text material '{textMaterial.Title}' was approved.";

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Text material approved",
                    Body = body,
                    Attachment = null
                });
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
        
        /// <summary>
        /// Sends a notification on user's email if their text material was rejected
        /// </summary>
        /// <param name="user">The author of text material</param>
        /// <param name="textMaterial">Text material that was recently rejected</param>
        /// <param name="rejectMessage">The reason why the text material was rejected</param>
        /// <exception cref="CardFileException"></exception>
        public void NotifyThatTextMaterialWasRejected(User user, TextMaterial textMaterial, string? rejectMessage = null)
        {
            var body = $"Hello {user.UserName}." +
                $"Your text material '{textMaterial.Title}' was rejected.\n" +
                $"Reason: {rejectMessage}";

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Text material rejected",
                    Body = body,
                    Attachment = null
                });
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
    }
}
