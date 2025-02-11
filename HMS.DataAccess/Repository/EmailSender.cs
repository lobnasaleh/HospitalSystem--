
using System.Net.Mail;
using System.Net;
using HMS.Entities.Interfaces;




namespace HMS.DataAccess.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = "lobnasalem24@outlook.com";
            var pass = "lobna123456";
            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail,pass)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true // Make sure the body is interpreted as HTML
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
    }

