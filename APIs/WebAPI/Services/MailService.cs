using System.Text;
using Application.Configuration;
using Application.Interfaces;
using Application.ViewModels.MailViewModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebAPI.Services
{
    public class MailService : IEmailService
    {
        private readonly MailConfigurations config;

        public MailService(IOptions<MailConfigurations> config)
        {
            this.config = config.Value;
        }

        public async Task<bool> SendAsync(MailDataModel mailData, CancellationToken cancellationToken)
        {
            var mail = new MimeMessage();
            #region Sender & Receiver
            //sender
            mail.From.Add(new MailboxAddress(config.DisplayName, mailData.From ?? config.From));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? config.DisplayName, mailData.From ?? config.From);

            //receiver
            foreach (var mailAddress in mailData.To)
            {
                mail.To.Add(MailboxAddress.Parse(mailAddress));
            }
            //set Reply to if specified in request
            if (!string.IsNullOrEmpty(mailData.ReplyTo))
            {
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));
            }

            // BCC
            // Check if a BCC was supplied in the request
            if (mailData.Bcc != null)
            {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            // CC
            // Check if a CC address was supplied in the request
            if (mailData.Cc != null)
            {
                foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }
            #endregion

            #region Content

            //add content to mime message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            #region Send Mail
            using var smtp = new SmtpClient();
            if (config.UseSSL)
            {
                await smtp.ConnectAsync(config.Host, config.Port, MailKit.Security.SecureSocketOptions.SslOnConnect, cancellationToken);
            }
            else if (config.UseStartTls)
            {
                await smtp.ConnectAsync(config.Host, config.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            }
            await smtp.AuthenticateAsync(config.UserName, config.Password, cancellationToken);
            await smtp.SendAsync(mail, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
            #endregion

            return true;
        }

        #region GetMailBody
        public string GetMailBody(string title = "", string speech = "", string mainContent = "", string alternativeSpeech = "",
            string alternativeContent = "", string sign = "", string mainContentLink = "")
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("HtmlTemplate/MailTemplate/Layout.html"))
            {
                string line = "";
                StringBuilder stringBuilder = new StringBuilder();
                while ((line = reader.ReadLine()) != null)
                {
                    stringBuilder.Append(line);
                }
                body = stringBuilder.ToString();
            }
            body = body.Replace("{Title}", title);
            body = body.Replace("{Speech}", speech);
            body = body.Replace("{MainContentLink}", mainContentLink);
            body = body.Replace("{MainContent}", mainContent);
            body = body.Replace("{AlternativeSpeech}", alternativeSpeech);
            body = body.Replace("{AlternativeContent}", alternativeContent);
            body = body.Replace("{Sign}", sign);
            return body;
        }
        #endregion
    }
}
