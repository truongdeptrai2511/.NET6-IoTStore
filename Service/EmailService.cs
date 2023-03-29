using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Service.IService;
using System.Net;
using System.Net.Mail;

namespace IotSupplyStore.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendMail(EmailDto emailDto)
        {
            var fromAddress = new MailAddress("tannguyen.dut8091@gmail.com", "IotDeviceStore.com");
            var toAddress = new MailAddress(emailDto.ToEmailAddress, emailDto.ToName);
            const string fromPassword = "dfxramejijwpjuch";
            string subject = emailDto.Subject;
            string body = emailDto.Body;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}
