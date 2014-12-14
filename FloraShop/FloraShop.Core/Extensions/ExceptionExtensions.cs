using FloraShop.Core.Configurations;
using FloraShop.Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Logs the specified exp using log4net
        /// </summary>
        /// <param name="exp">The exp.</param>
        public static void Log(this Exception exp)
        {
            if (exp != null)
            {
                var cmsEx = new SiteException(exp);
                CMSLog(cmsEx);
            }
        }

        /// <summary>
        /// Logs the specified exp using log4net
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="message">The message.</param>
        public static void Log(this Exception exp, string message)
        {
            if (exp != null)
            {
                var msg = Globals.HtmlEncode(message);
                msg = Globals.EncodeToUtf8(msg);

                var cmsEx = new SiteException(exp, msg);
                CMSLog(cmsEx);
            }
        }

        /// <summary>
        /// Logs the specified exp using log4net
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="type">The type.</param>
        public static void Log(this Exception exp, SiteExceptionType type)
        {
            if (exp != null)
            {
                var cmsEx = new SiteException(exp, string.Empty, type);
                CMSLog(cmsEx);
            }
        }

        /// <summary>
        /// Logs the specified exp using log4net
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public static void Log(this Exception exp, SiteExceptionType type, string message)
        {
            if (exp != null)
            {
                var msg = Globals.HtmlEncode(message);
                msg = Globals.EncodeToUtf8(msg);

                var cmsEx = new SiteException(exp, msg, type);
                CMSLog(cmsEx);
            }
        }

        private static void CMSLog(SiteException exp)
        {
            if (SiteConfiguration.GetConfig().LogException == false)
                return;

            // var provider = IoC.Kernel.Get<LogProvider>();
            var provider = new LogProvider();
            provider.LogException(exp, 1);
        }
    }
}
