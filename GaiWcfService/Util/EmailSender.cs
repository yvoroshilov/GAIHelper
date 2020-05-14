using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Util {
    public class EmailSender {
        private static readonly object padlock = new object();

        private static EmailSender instance = null;
        public static EmailSender Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new EmailSender();
                    }
                    return instance;
                }
            }
        }

        private readonly SmtpClient smtpClient;
        private readonly string FROM_ADDRESS;
        private MyLogger logger = MyLogger.Instance;

        private EmailSender() {
            smtpClient = new SmtpClient("smtp.mail.ru", 587);
            smtpClient.Credentials = new NetworkCredential(Configuration.MailLogin, Configuration.MailPassword);
            smtpClient.EnableSsl = true;

            FROM_ADDRESS = Configuration.MailLogin;
        }

        public void SendMail(string to, string subject, string text) {
            try {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FROM_ADDRESS);
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = text;
                smtpClient.SendMailAsync(mailMessage).Wait();
                mailMessage.Dispose();
                logger.Write("EMAIL SENT");
            } catch (Exception e) {
                logger.Write(e.ToString());
            }
        }

    }
}
