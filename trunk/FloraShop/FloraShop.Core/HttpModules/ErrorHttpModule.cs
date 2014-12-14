using FloraShop.Core.Configurations;
using FloraShop.Core.Extensions;
using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FloraShop.Core.HttpModules
{
    public class ErrorHttpModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.AuthenticateRequest += context_AuthenticateRequest;
            context.Error += context_Error;
            context.AuthorizeRequest += context_AuthorizeRequest;
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, EventArgs e)
        {
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
        }

        void context_Error(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            SiteException exp = null;
            if (context.Server.GetLastError() != null)
                exp = new SiteException(context.Server.GetLastError());

            if (exp != null)
            {
                int errorCode = 404; // No error
                var httpException = context.Server.GetLastError() as HttpException;
                if (httpException != null)
                    errorCode = httpException.GetHttpCode();

                var config = SiteContext.Current.SiteConfig;
                if (errorCode != 200)
                {
                    if (config.ErrorEmail.FilterErrorCode.Contains("*") || config.ErrorEmail.FilterErrorCode.Contains(errorCode.ToString()))
                        SendErrorEmail(config, exp);
                }

            }
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
        }

        #region send error email

        void SendErrorEmail(SiteConfiguration config, SiteException ex)
        {
            if (!config.ErrorEmail.IsSendEmail)
                return;

            try
            {
                MailMessage msg = new MailMessage();
                string[] addresses = config.ErrorEmail.DefaultReceiver.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                Regex regr = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

                foreach (string adr in addresses)
                {
                    if (regr.IsMatch(adr.Trim()))
                    {
                        msg.To.Add(adr);
                    }
                }

                msg.From = new MailAddress(config.ErrorEmail.DefaultSender);
                msg.Subject = config.ErrorEmail.SubjectPrefix + HttpContext.Current.Request.Url.Host;
                msg.Body = GetErrorDetail(ex);
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.SubjectEncoding = Encoding.UTF8;

                SmtpClient mailClient = new SmtpClient();

                if (config.ErrorEmail.SmtpAuthentication)
                {
                    System.Net.NetworkCredential credential = new System.Net.NetworkCredential(config.ErrorEmail.SmtpUsername, config.ErrorEmail.SmtpPassword);
                    mailClient.Credentials = credential;
                }

                mailClient.Host = config.ErrorEmail.SmtpServer;
                mailClient.Port = config.ErrorEmail.SmtpPort;
                mailClient.EnableSsl = config.ErrorEmail.EnableSsl;

                mailClient.Send(msg);
            }
            catch (Exception exp)
            {
                exp.Log();
            }
        }

        string GetErrorDetail(Exception ex)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine();

            if (HttpContext.Current.Request.Url != null)
                msg.AppendLine("<b>Error page:</b> " + HttpContext.Current.Request.Url.ToString());

            if (HttpContext.Current.Request.UrlReferrer != null)
                msg.AppendLine("<b>Previous page:</b> " + HttpContext.Current.Request.UrlReferrer.ToString());

            if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
                msg.AppendLine("<b>Platform:</b> " + HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString());

            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                msg.AppendLine("<b>User IP:</b> " + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());

            msg.AppendLine("<b>User name:</b> " + (HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : "Anonymous"));
            msg.AppendLine("<b>Error on date:</b> " + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

            if (ex != null)
            {
                Exception inner = ex.InnerException;
                int count = 5;
                while (count > 0 && inner != null)
                {
                    msg.AppendLine("<b>Message:</b> " + inner.Message);
                    msg.AppendLine("<b>Exception type:</b> " + inner.GetType().ToString());
                    msg.AppendLine("<b>Exception detail:</b>");
                    msg.AppendLine(inner.Source);
                    msg.AppendLine(inner.StackTrace);

                    inner = inner.InnerException;
                    count--;
                }
            }
            else
                msg.AppendLine("No information about the exception. Exception is null");

            return msg.ToString().Replace(System.Environment.NewLine, "<br/>");

        }

        #endregion
    }
}
