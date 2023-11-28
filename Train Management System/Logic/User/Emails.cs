using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// deprecated / cant't be bothered to attempt /fix
/// </summary>

namespace Logic.User
{
    public class Emails
    {
        private readonly IConfiguration _configuration;

        public Emails(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendEmailAsync()
        {
            string name = _configuration.GetSection("MailboxAddress").GetSection("Name").Value;
            string address = _configuration.GetSection("MailboxAddress").GetSection("Address").Value;

            var messageToSend = new MimeMessage
            {
                Sender = new MailboxAddress(name, address),
                Subject = "Your Subject",
            };
            if (messageToSend == null) throw new ArgumentNullException(nameof(messageToSend));

            messageToSend.Body = new TextPart(TextFormat.Html) { Text = "Dit is een test" };
            messageToSend.To.Add(new MailboxAddress("MailboxAddress", "489971@student.fontys.nl"));
            messageToSend.From.Add(new MailboxAddress("MailboxAddress", address)); //important!!

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.MessageSent += (sender, args) =>
                {
                    Console.WriteLine(args.Response);
                };
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await smtp.ConnectAsync("mailrelay.fhict.local", 25, SecureSocketOptions.None);
                //await smtp.AuthenticateAsync("Username", "Password");
                await smtp.SendAsync(messageToSend);
                await smtp.DisconnectAsync(true);
            };
        }
    }
}
