namespace HealthManager.Services.Mail
{
    public interface IMailService
    {
        public void SendAppointmentConfirmationMail(MailDTO mailProfile, byte[] pdfBytes);
    }
}
