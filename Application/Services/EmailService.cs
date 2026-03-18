using Infrastructure.Abstracts;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Infrastructure.Configurations;

namespace Application.Services
{
    public class EmailService : SmtpClient, IEmailService
    {
        private readonly EmailServiceOptions options;

        public EmailService(IOptions<EmailServiceOptions> options)
        {
            this.options = options.Value;


            Host = this.options.SmtpServer;
            Port = this.options.SmtpPort;
            Credentials = new NetworkCredential(this.options.UserName, this.options.Password);
            EnableSsl = true;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            //MailMessage message = new MailMessage();
            //message.From = new MailAddress(options.UserName, options.DisplayName);
            //message.To.Add(to);
            //message.Subject = subject;

            //message.Body = body;
            //message.IsBodyHtml = true;

            //await SendMailAsync(message);

            using var message = new MailMessage
            {
                From = new MailAddress(options.UserName, options.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            using var client = new SmtpClient(options.SmtpServer, options.SmtpPort)
            {
                Credentials = new NetworkCredential(options.UserName, options.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

        public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, int chunkSize = 80)
        {
            var emailList = recipients.ToList();
            var chunks = emailList
                .Select((email, index) => new { email, index })
                .GroupBy(x => x.index / chunkSize)
                .Select(g => g.Select(x => x.email).ToList())
                .ToList();

            var tasks = chunks.Select(chunk => Task.Run(async () =>
            {
                using var client = new SmtpClient(options.SmtpServer, options.SmtpPort)
                {
                    Credentials = new NetworkCredential(options.UserName, options.Password),
                    EnableSsl = true
                };

                foreach (var to in chunk)
                {
                    using var msg = new MailMessage
                    {
                        From = new MailAddress(options.UserName, options.DisplayName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    msg.To.Add(to);

                    try
                    {
                        await client.SendMailAsync(msg);
                    }
                    catch (Exception ex)
                    {
                        // Handle or log individual failures
                        Console.Error.WriteLine($"Error sending to {to}: {ex.Message}");
                    }
                }
            })).ToList();

            await Task.WhenAll(tasks);
        }

    }
}
