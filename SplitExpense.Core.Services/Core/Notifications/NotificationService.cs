using Microsoft.Extensions.Configuration;
using SplitExpense.Core.Models.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services.Core.Notifications
{
    public class NotificationService
    {
        private readonly IConfiguration configuration;
        private readonly string emailDomain;
        private readonly int emailPort;
        private readonly string emailFrom;
        private readonly string emailFromPassword;

        public NotificationService(IConfiguration configuration)
        {
            this.configuration = configuration;
            var settingSection = this.configuration.GetSection("EmailSettings");
            this.emailDomain = settingSection["EmailDomain"];
            this.emailPort = Convert.ToInt32(settingSection["EmailPort"]);
            this.emailFrom = settingSection["EmailFrom"];
            this.emailFromPassword = settingSection["Password"];
        }

        public void TriggerNotification(EmailNotification email)
        {
            if(email != null && email.To.Any())
            {
                using(MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("no-reply@gmail.com", "Split Expense");

                    email.To.ForEach(e =>
                    {
                        mail.To.Add(e);
                    });
                    
                    if(email.CC.Any())
                    {
                        email.CC.ForEach(e =>
                        {
                            mail.To.Add(e);
                        });
                    }

                    mail.Subject = email.Subject;
                    mail.Body = email.Body;
                    mail.IsBodyHtml = email.IsHTML;

                    this.TriggerEmail(mail);
                }
            }
        }

        public void TriggerNotifications(IList<EmailNotification> emails)
        {
            if(emails.Any())
            {
                var mails = emails.Select(e =>
                {
                    var mailMessage = new MailMessage()
                    {
                        From = new MailAddress("no-reply@gmail.com", "Split Expense"),
                        Subject = e.Subject,
                        Body = e.Body,
                        IsBodyHtml = e.IsHTML
                    };

                    e.To.ForEach(toEmail =>
                    {
                        mailMessage.To.Add(toEmail);
                    });

                    e.CC.ForEach(ccEmail =>
                    {
                        mailMessage.CC.Add(ccEmail);
                    });


                    return mailMessage;
                }).ToList();

                this.TriggerEmailBulk(mails);
            }
        }

        private void TriggerEmail(MailMessage mail)
        {
            using (SmtpClient smtp = new SmtpClient(this.emailDomain, this.emailPort))
            {
                smtp.Credentials = new NetworkCredential(this.emailFrom, this.emailFromPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        private void TriggerEmailBulk(List<MailMessage> mails)
        {
            using (SmtpClient smtp = new SmtpClient(this.emailDomain, this.emailPort))
            {
                smtp.Credentials = new NetworkCredential(this.emailFrom, this.emailFromPassword);
                smtp.EnableSsl = true;
                mails.ForEach(mail =>
                {
                    smtp.Send(mail);
                });
            }
        }
    }
}
