namespace HealthManager.Services.Mail
{
    public class MailDTO
    {
        public string DestinataryMail { get; set; } = null!;
        public string DestinataryName { get; set; } = null!;
        public string MailTitle { get; set; } = null!;
        public string MailSubject { get; set; } = null!;
    }
}
