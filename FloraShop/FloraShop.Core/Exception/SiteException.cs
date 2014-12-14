using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Text;
using FloraShop.Core;
using log4net;

namespace FloraShop.Core
{
    public class SiteException : Exception
    {
        public SiteException(Exception ex) :
            this(ex, string.Empty, SiteExceptionType.UnknownError)
        {

        }

        public SiteException(Exception ex, string errorMessage) :
            this(ex, errorMessage, SiteExceptionType.UnknownError)
        {

        }

        public SiteException(Exception inner, string errorMessage, SiteExceptionType type)
            : base(errorMessage, inner)
        {
            Init();
            ExceptionType = type;
        }


        public SiteExceptionType ExceptionType
        {
            get;
            set;
        }


        #region Public methods
        public override int GetHashCode()
        {
            return this.GetHashCode(SiteContext.Current.GetHashCode());
        }

        public int GetHashCode(int settingsID)
        {
            string stringToHash = (settingsID + ExceptionType + this.HttpPathAndQuery + this.ToString());

            return stringToHash.GetHashCode();
        }

        //public void Log(int settingsID)
        //{
        //    ILog log = new LogManager();
        //    try
        //    {
        //        if (CMSConfiguration.GetConfig().EnableExceptionLogging)
        //            CMSLogDataProvider.Instance().LogException(this, settingsID);
        //    }
        //    catch { }
        //}

        //public void Log()
        //{
        //    try
        //    {
        //        if (ClawContext.Current != null)
        //            Log(ClawContext.Current.SiteSettings.SettingsID);
        //        else 
        //            Log(-1);
        //    }
        //    catch
        //    {
        //        Log(-1);
        //    }
        //}

        #endregion

        //public override string ToString()
        //{
        //    StringBuilder result = new StringBuilder();
        //    result.AppendFormat("{0}: {1}", this.ExceptionType.ToString(), this.Message);
        //    result.AppendLine();

        //    if (ExceptionType == CMSExceptionType.PostAccessDenied)
        //        result.AppendLine(string.Format("{0}{1}", base.ToString(), new System.Diagnostics.StackTrace()));
        //    else
        //    {
        //        result.AppendLine(string.Format("{0} {1}", base.ToString(), this.StackTrace));
        //        int stackTraceLevel = 5;
        //        Exception inner = InnerException;
        //        while (inner != null && stackTraceLevel-- > 0)
        //        {
        //            result.AppendLine(string.Format("{0}: {1}", inner.Source, inner.Message));
        //            result.AppendLine(string.Format("{0} {1}", base.ToString(), this.StackTrace));
        //            inner = inner.InnerException;
        //        }
        //    }

        //    return result.ToString();
        //}

        #region Public Properties

        /// <summary>
        /// Gets or sets the current user id.
        /// </summary>
        /// <value>The current user id.</value>
        public string CurrentUserId
        {
            get;
            set;
        }

        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent
        {
            get;
            set;
        }

        public string IPAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP referrer.
        /// </summary>
        /// <value>The HTTP referrer.</value>
        public string HttpReferrer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP verb.
        /// </summary>
        /// <value>The HTTP verb.</value>
        public string HttpVerb
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP path and query.
        /// </summary>
        /// <value>The HTTP path and query.</value>
        public string HttpPathAndQuery
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date last occurred.
        /// </summary>
        /// <value>The date last occurred.</value>
        public DateTime DateLastOccurred
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exception ID.
        /// </summary>
        /// <value>The exception ID.</value>
        public string ExceptionID
        {
            get;
            set;
        }


        #endregion

        #region Private helper functions

        void Init()
        {
            try
            {
                var ctx = SiteContext.Current;

                if (ctx != null &&
                    ctx.Context != null &&
                    ctx.Context.Request != null)
                {
                    if (ctx.IsWebRequest && !string.IsNullOrEmpty(ctx.User.Username))
                        CurrentUserId = ctx.User.Id.ToString();

                    if (ctx.Context.Request.UrlReferrer != null)
                        HttpReferrer = ctx.Context.Request.UrlReferrer.ToString();

                    if (ctx.Context.Request.UserAgent != null)
                        UserAgent = ctx.Context.Request.UserAgent;

                    if (ctx.Context.Request.UserHostAddress != null)
                        IPAddress = ctx.Context.Request.UserHostAddress;

                    // ACS HttpRequest.RequestType can throw a null reference exception
                    // and I can't find any way to test if it will.
                    // This happens when the request's inner HttpWorkerRequest is null.
                    // This is observable when the exception is created from the 
                    // ForumsHttpModule.ScheduledWorkCallbackEmailInterval method's catch block.
                    // I assume this is because it happens on timer/backgroundthread.
                    if (ctx.Context.Request != null
                        && ctx.Context.Request.RequestType != null)
                        HttpVerb = ctx.Context.Request.RequestType;

                    // ACS "forumContext.Context.Request.Url != null" check was added because
                    // , similarly to above, the Url property will be null when this method is called
                    // from the ForumsHttpModule.ScheduledWorkCallbackEmailInterval timer callback.
                    if (ctx.Context.Request != null
                        && ctx.Context.Request.Url != null
                        && ctx.Context.Request.Url.PathAndQuery != null)
                        HttpPathAndQuery = ctx.Context.Request.Url.PathAndQuery;

                }

                DateCreated = DateTime.Now;
                DateLastOccurred = DateTime.Now;
            }
            catch { }
        }

        #endregion
    }
}
