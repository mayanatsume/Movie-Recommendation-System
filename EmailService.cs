using System.Net;
using System.Net.Mail;

namespace MovieBookRecommendation.Services
{
    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPassword;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPassword)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUser = smtpUser;
            this.smtpPassword = smtpPassword;
        }

        public void SendSuggestionsEmail(string toEmail, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(smtpUser);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);
            }
        }
    }
}
