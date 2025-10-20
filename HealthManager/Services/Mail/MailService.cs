using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using MimeKit;

namespace HealthManager.Services.Mail
{
    public class MailService : IMailService
    {
        private IConfiguration _config;
        public MailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendAppointmentConfirmationMail(MailDTO mailProfile, byte[] pdfBytes)
        {
            string rutaHtml = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Mail", "mailprofile.html");
            string htmlFile = File.ReadAllText(rutaHtml);

            var mailTemplate = new MimeMessage();

            mailTemplate.From.Add(new MailboxAddress("HealthManager Project", _config.GetSection("Email:Username").Value));
            mailTemplate.To.Add(new MailboxAddress(mailProfile.DestinataryName, mailProfile.DestinataryMail));
            mailTemplate.Subject = mailProfile.MailSubject;
            mailTemplate.Body = new TextPart("plain")
            {
                Text = ""
            };

            var builder = new BodyBuilder
            {
                HtmlBody = htmlFile
            };

            

            builder.Attachments.Add(
            "Comprobante.pdf",
            pdfBytes,
            new ContentType("application", "pdf")
        );

            mailTemplate.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:PassWord").Value);

            smtp.Send(mailTemplate);

            smtp.Disconnect(true);
        }
    }
}
