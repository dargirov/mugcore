using Microsoft.Extensions.Configuration;
using MugStore.Data.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MugStore.Services.Common
{
    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendMailAsync(Order order)
        {
            var enabled = this.configuration.GetSection("AppSettings")["OrderConfirmSendMailEnabled"];
            if (!bool.Parse(enabled))
            {
                return;
            }

            var toEmail = order.DeliveryInfo.Email;
            var subject = $"Потвърждение на поръчка #{order.Acronym}";
            var body = $"<p>Здравейте {order.DeliveryInfo.FullName}</p>,<p>успешно направите поръчка #{order.Acronym}.</p><p>Повече информация може да видите на http://mug3.eu/o/{order.Acronym}</p>";

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            var mailSettings = this.configuration.GetSection("MailSettings");

            var message = new MailMessage();
            var smtp = new SmtpClient();
            message.From = new MailAddress(mailSettings["Mail"], mailSettings["DisplayName"]);
            message.To.Add(new MailAddress(toAddress));
            message.Subject = subject;
            message.IsBodyHtml = false;
            message.Body = body;
            smtp.Port = int.Parse(mailSettings["Port"]);
            smtp.Host = mailSettings["Host"];
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mailSettings["Mail"], mailSettings["Password"]);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }
    }
}
