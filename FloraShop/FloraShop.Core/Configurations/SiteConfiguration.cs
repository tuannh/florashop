using FloraShop.Core.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Xml.Serialization;

namespace FloraShop.Core.Configurations
{
    [DataContract]
    public class SiteConfiguration
    {
        #region utility methods

        private const string CacheKey = "Configuration";

        public static SiteConfiguration GetConfig()
        {
            var config = SiteCache.Get<SiteConfiguration>(SiteConfiguration.CacheKey);
            if (config == null)
            {
                string path = Globals.MapPath("~/site.config");

                if (!File.Exists(path))
                    path = Globals.MapPath("~/config/site.config");

                if (!File.Exists(path))
                {
                    config = new SiteConfiguration();
                    SerializationUtility.Serialize<SiteConfiguration>(config, path);
                }
                else
                {
                    config = SerializationUtility.Deserialize<SiteConfiguration>(path);
                }

                var dep = new CacheDependency(path);
                SiteCache.Insert(SiteConfiguration.CacheKey, config, dep);
            }

            return config;
        }

        public static void SaveConfig(SiteConfiguration config)
        {
            if (config != null)
            {
                string path = Globals.MapPath("~/Claw.config");

                if (File.Exists(path))
                {
                    SerializationUtility.Serialize<SiteConfiguration>(config, path);
                }
                else
                {
                    path = Globals.MapPath("~/config/Claw.config");

                    if (File.Exists(path))
                        SerializationUtility.Serialize<SiteConfiguration>(config, path);
                }
            }
        }

        public SiteConfiguration()
        {
            LogException = true;
            LogEvent = true;
            RemoveWhitespaces = true;

            ConfigFile = new ConfigFile()
            {
                Log = "~/config/log4net.config",
                Route = "~/config/routes.config",
            };

            ErrorEmail = new ErrorEmail()
            {
                IsSendEmail = true,
                SmtpServer = "mail.cateno.no",
                SmtpUsername = "smtp@cateno.no",
                SmtpPassword = "2008-KL",
                SmtpPort = 25,
                SmtpAuthentication = false,
                EnableSsl = false,
                DefaultSender = "no-reply@cateno.no",
                DefaultReceiver = "tuannh@cateno.no",
                SubjectPrefix = "Error on site: ",
                FilterErrorCode = "*"
            };

            Quality = 80;

            Product = new ImageResize()
            {
                Width = 2014,
                Height = 768,
                ThumbWidth = 200,
                ThumbHeight = 150,
                Background = "#FFF"
            };

            IsSendEmailToAdmin = true;
            IsSendEmailToUser = true;
            DefaultSender = "no-reply@florafashion.com.vn";
            AdminEmail = "nht257@yahoo.com";
        }

        #endregion

        #region properties

        [DataMember(Order = 1)]
        public bool LogException
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public bool LogEvent
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public bool RemoveWhitespaces
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public ConfigFile ConfigFile
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public ErrorEmail ErrorEmail
        {
            get;
            set;
        }

        [DataMember(Order = 6)]
        public bool AddTrailingSlash { get; set; }

        [DataMember(Order = 7)]
        public int Quality { get; set; }

        [DataMember(Order = 8)]
        public ImageResize Banner { get; set; }

        [DataMember(Order = 9)]
        public ImageResize Brand { get; set; }

        [DataMember(Order = 10)]
        public ImageResize Product { get; set; }

        [DataMember(Order = 11)]
        public ImageResize News { get; set; }

        [DataMember(Order = 12)]
        public ImageResize UserGuide { get; set; }

        [DataMember(Order = 13)]
        public bool IsSendEmailToAdmin { get; set; }

        [DataMember(Order = 14)]
        public bool IsSendEmailToUser { get; set; }

        [DataMember(Order = 15)]
        public string DefaultSender { get; set; }

        [DataMember(Order = 16)]
        public string AdminEmail { get; set; }

        #endregion
    }

    public class ErrorEmail
    {
        public bool IsSendEmail
        {
            get;
            set;
        }

        public string SmtpServer { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public int SmtpPort { get; set; }

        public bool SmtpAuthentication { get; set; }

        public bool EnableSsl { get; set; }

        public string DefaultSender { get; set; }

        public string DefaultReceiver { get; set; }

        public string SubjectPrefix { get; set; }

        /// <summary>
        /// set * to send all error email. Using ";" or "," for splitter multi error code.
        /// </summary>
        public string FilterErrorCode { get; set; }
    }

    public class ConfigFile
    {
        public string Log
        {
            get;
            set;
        }

        public string Route
        {
            get;
            set;
        }
    }

    public class ImageResize
    {
        /// <summary>
        /// Gets or sets the background.  Default color: #FFF
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        [DataMember(Order = 1)]
        public string Background { get; set; }

        [DataMember(Order = 2)]
        public int Width { get; set; }

        [DataMember(Order = 3)]
        public int Height { get; set; }

        [DataMember(Order = 4)]
        public int ThumbWidth { get; set; }

        [DataMember(Order = 5)]
        public int ThumbHeight { get; set; }
    }
}
