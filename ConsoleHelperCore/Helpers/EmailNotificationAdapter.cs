using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelperCore.Helpers
{
    public class EmailNotificationAdapter
    {
        private readonly Microsoft.Exchange.WebServices.Data.ExchangeService exchangeSvc;

        public EmailNotificationAdapter(string emailServiceEndpoint, string emailSvcUserAccount, string emailSvcUserPassword, string emailSvcUserDomain)
        {
            if (string.IsNullOrWhiteSpace(emailServiceEndpoint))
            {
                throw new ArgumentException("Service end point must not be null or whitespace.", nameof(emailServiceEndpoint));
            }

            if (string.IsNullOrWhiteSpace(emailSvcUserAccount))
            {
                throw new ArgumentException("User account must not be null or whitespace.", nameof(emailSvcUserAccount));
            }

            if (string.IsNullOrWhiteSpace(emailSvcUserPassword))
            {
                throw new ArgumentException("User assword must not be null or whitespace.", nameof(emailSvcUserPassword));
            }

            if (string.IsNullOrWhiteSpace(emailSvcUserDomain))
            {
                throw new ArgumentException("User domain must not be null or whitespace.", nameof(emailSvcUserDomain));
            }

            exchangeSvc = new Microsoft.Exchange.WebServices.Data.ExchangeService(Microsoft.Exchange.WebServices.Data.ExchangeVersion.Exchange2013)
            {
                Url = new Uri(emailServiceEndpoint),
                Credentials = new Microsoft.Exchange.WebServices.Data.WebCredentials(emailSvcUserAccount, emailSvcUserPassword, emailSvcUserDomain)
            };
        }

        private Task SendMailAsync(string subject, string body, IReadOnlyList<string> toRecipients, IReadOnlyList<string> ccRecipients = null, IDictionary<string, byte[]> attachments = null)
        {
            return Task.Run(() =>
            {
                var message = new Microsoft.Exchange.WebServices.Data.EmailMessage(exchangeSvc);

                foreach (string toRecipient in toRecipients)
                {
                    message.ToRecipients.Add(NormailizeEmailRecipient(toRecipient));
                }

                if (ccRecipients != null)
                {
                    foreach (string ccRecipient in ccRecipients)
                    {
                        message.CcRecipients.Add(NormailizeEmailRecipient(ccRecipient));
                    }
                }

                message.Subject = subject;
                message.Body = body;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        var fileAttachment = message.Attachments.AddFileAttachment(attachment.Key, attachment.Value);
                        fileAttachment.IsInline = true;
                        fileAttachment.ContentId = attachment.Key;
                    }
                }

                message.Save();
                message.Send();
            });
        }

        private static string NormailizeEmailRecipient(string recipient) => recipient.Trim();
    }
}
