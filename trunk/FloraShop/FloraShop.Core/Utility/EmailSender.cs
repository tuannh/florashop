using FloraShop.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FloraShop.Core.Interfaces;
using System.IO;
using FloraShop.Core.Configurations;
using FloraShop.Core.Extensions;

namespace FloraShop.Core.Utility
{
    /// <summary>
    /// Send email utility class
    /// Write by tuannh On 2013.09.06
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Gets the current system email setting.
        /// </summary>
        /// <returns></returns>
        private static SmtpClient GetCurrentSystemEmailSetting()
        {
            var settings = SiteConfiguration.GetConfig().ErrorEmail;
            var mailServer = new SmtpClient(settings.SmtpServer, settings.SmtpPort);

            mailServer.EnableSsl = settings.EnableSsl;

            if (settings.SmtpAuthentication)
            {
                var authentication = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);
                mailServer.Credentials = authentication;
            }

            return mailServer;
        }

        #region send email directly

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="from">From email address.</param>
        /// <param name="to">A string contains a list To email address. Each email is separeated by "," or ";"</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, string from, string to)
        {
            return InstantSend(subject, body, true, from, to);
        }

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="from">From email address.</param>
        /// <param name="to">A string contains a list To email address. Each email is separeated by "," or ";"</param>
        /// <param name="cc">A string contains a list CC email address. Each email is separeated by "," or ";"</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, string from, string to, string cc)
        {
            return InstantSend(subject, body, true, from, to, cc, string.Empty, string.Empty, Encoding.UTF8, Encoding.UTF8, null);
        }

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isHtmlBody">if set to <c>true</c> [is HTML body].</param>
        /// <param name="from">From email address.</param>
        /// <param name="to">A string contains a list To email address. Each email is separeated by "," or ";"</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, bool isHtmlBody, string from, string to)
        {
            return InstantSend(subject, body, isHtmlBody, from, to, string.Empty, string.Empty, string.Empty, Encoding.UTF8, Encoding.UTF8, null);
        }

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isHtmlBody">if set to <c>true</c> [is HTML body].</param>
        /// <param name="from">From email address.</param>
        /// <param name="to">A string contains a list To email address. Each email is separeated by "," or ";"</param>
        /// <param name="attachFiles">The full path attach files.</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, bool isHtmlBody, string from, string to, string[] attachFiles)
        {
            return InstantSend(subject, body, isHtmlBody, from, to, string.Empty, string.Empty, string.Empty, Encoding.UTF8, Encoding.UTF8, attachFiles);
        }

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isHtmlBody">if set to <c>true</c> [is HTML body].</param>
        /// <param name="from">From email address.</param>
        /// <param name="to">A string contains a list To email address. Each email is separeated by "," or ";"</param>
        /// <param name="displayNameFrom">The display name of from email.</param>
        /// <param name="dislayNameTo">The dislay name of to email.</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, bool isHtmlBody, string from, string to, string displayNameFrom, string dislayNameTo)
        {
            return InstantSend(subject, body, isHtmlBody, from, to, string.Empty, displayNameFrom, dislayNameTo, Encoding.UTF8, Encoding.UTF8, null);
        }

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isHtmlBody">if set to <c>true</c> [is HTML body].</param>
        /// <param name="from">From email address.</param>
        /// <param name="to">A string contains a list To email address. Each email is separeated by "," or ";"</param>
        /// <param name="cc">A string contains a list CC email address. Each email is separeated by "," or ";"</param>
        /// <param name="displayNameFrom">The display name of from email.</param>
        /// <param name="dislayNameTo">The dislay name of to email.</param>
        /// <param name="subjectEncoding">The subject encoding.</param>
        /// <param name="bodyEncoding">The body encoding.</param>
        /// <param name="attachFiles">The full path attach files.</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, bool isHtmlBody, string from, string to, string cc, string displayNameFrom, string dislayNameTo, Encoding subjectEncoding, Encoding bodyEncoding, string[] attachFiles)
        {
            return InstantSend(subject, body, isHtmlBody, from, to, cc, null, displayNameFrom, dislayNameTo, Encoding.UTF8, Encoding.UTF8, null);
        }

        /// <summary>
        /// Send email immediately. Return empty if success or error message if error
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isHtmlBody">if set to <c>true</c> [is HTML body].</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="displayNameFrom">The display name from.</param>
        /// <param name="dislayNameTo">The dislay name to.</param>
        /// <param name="subjectEncoding">The subject encoding.</param>
        /// <param name="bodyEncoding">The body encoding.</param>
        /// <param name="attachFiles">The attach files.</param>
        /// <returns></returns>
        public static string InstantSend(string subject, string body, bool isHtmlBody, string from, string to, string cc, string bcc, string displayNameFrom, string dislayNameTo, Encoding subjectEncoding, Encoding bodyEncoding, string[] attachFiles)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(from, displayNameFrom);
            mail.IsBodyHtml = isHtmlBody;
            mail.Subject = subject;
            mail.SubjectEncoding = subjectEncoding;
            mail.BodyEncoding = bodyEncoding;

            var regr = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            #region To email

            string[] addresses = to.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);

            if (addresses.Length == 1)
            {
                mail.To.Add(new MailAddress(addresses[0], dislayNameTo));
            }
            else
            {
                foreach (string adr in addresses)
                {
                    if (regr.IsMatch(adr.Trim()))
                        mail.To.Add(adr);
                }
            }

            #endregion

            #region CC email

            if (!string.IsNullOrEmpty(cc))
            {
                string[] ccAddresses = cc.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                if (ccAddresses != null && ccAddresses.Length > 0)
                {
                    foreach (string adr in ccAddresses)
                    {
                        if (regr.IsMatch(adr.Trim()))
                            mail.CC.Add(adr);
                    }
                }
            }

            #endregion

            #region BCC email

            if (!string.IsNullOrEmpty(bcc))
            {
                string[] bccAddresses = bcc.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                if (bccAddresses != null && bccAddresses.Length > 0)
                {
                    foreach (string adr in bccAddresses)
                    {
                        if (regr.IsMatch(adr.Trim()))
                            mail.Bcc.Add(adr);
                    }
                }
            }

            #endregion

            #region Attach files

            if (attachFiles != null)
            {
                foreach (var attachFile in attachFiles)
                {
                    var path = attachFile ?? "";

                    if (path.StartsWith("~/"))
                        path = Globals.MapPath(path);

                    if (File.Exists(path))
                    {
                        var item = new Attachment(path);
                        mail.Attachments.Add(item);
                    }
                }
            }

            #endregion

            #region email body

            // Send email with plain text
            if (!mail.IsBodyHtml)
            {
                AlternateView plainText = AlternateView.CreateAlternateViewFromString(body, Encoding.GetEncoding("iso-8859-1"), "text/plain");
                plainText.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
                mail.AlternateViews.Add(plainText);
            }
            else
            {
                mail.Body = body;
            }

            #endregion

            var smtpClient = GetCurrentSystemEmailSetting();
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception exp)
            {
                exp.Log();
                return exp.ToString();
            }

            return string.Empty;
        }

        #endregion
    }
}
