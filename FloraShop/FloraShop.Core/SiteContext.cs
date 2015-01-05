/*
 * Created by: TuanNH
 * Created date: 2013.07.08
 * Description: Some utilities method for current login user
 * 
 */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Globalization;
using System.Web.UI;
using System.Web.Routing;
using FloraShop.Core.Configurations;
using FloraShop.Core.Utility;
using System.Web.Caching;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using FloraShop.Core.Extensions;
using System.Web.Mvc;
using FloraShop.Core.Domain;
using FloraShop.Core.DAL;
using Ninject;
using System.Data.Entity;

namespace FloraShop.Core
{
    public sealed class SiteContext
    {
        #region init CMS Context

        static readonly string dataKey = "SiteContextStore";

        private SiteContext() { }

        private SiteContext(HttpContext context)
        {
            if (context != null)
            {
                Context = context;
                if (context.Request != null)
                {
                    QueryString = context.Request.QueryString;
                    RawUrl = context.Request.RawUrl;
                }

                DbContext = IoC.Kernel.Get<FloraShopContext>();
            }
        }

        /// <summary>
        /// Returns the current instance of the SiteContext from the ThreadData Slot. If one is not found and a valid HttpContext can be found,
        /// it will be used. Otherwise, an exception will be thrown. 
        /// </summary>
        public static SiteContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;

                if (httpContext != null)
                {
                    if (httpContext.Items.Contains(dataKey))
                        return httpContext.Items[dataKey] as SiteContext;
                    else
                    {
                        SiteContext context = new SiteContext(httpContext);
                        if (context.Context != null)
                            context.Context.Items[dataKey] = context;

                        return context;
                    }
                }

                return new SiteContext();
            }
        }

        #endregion

        /// <summary>
        /// Gets an instance of data service for access domain object data
        /// </summary>
        /// <value>
        /// The data service.
        /// </value>
        public FloraShopContext DbContext
        {
            get;
            private set;
        }

        public NameValueCollection QueryString
        {
            get;
            set;
        }

        public string RawUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current site URL from request
        /// </summary>
        /// <value>
        /// return current site URL.
        /// </value>
        public string SiteUrl
        {
            get
            {
                string siteUrl = string.Empty;

                if (Context != null)
                {
                    siteUrl = string.Format("{0}://{1}", Context.Request.Url.Scheme, SiteDomain);
                    if (Context.Request.Url.Port != 80)
                        siteUrl = string.Format("{0}://{1}:{2}", Context.Request.Url.Scheme, SiteDomain, Context.Request.Url.Port);
                }

                return siteUrl;
            }
        }

        /// <summary>
        /// Gets the current site domain from request
        /// </summary>
        /// <value>
        /// The site domain.
        /// </value>
        public string SiteDomain
        {
            get
            {
                string siteDomain = string.Empty;

                if (Context != null)
                {
                    string hostName = Context.Request.Url.Host.Replace("www.", string.Empty);
                    string applicationPath = Context.Request.ApplicationPath;

                    if (applicationPath.EndsWith("/"))
                        applicationPath = applicationPath.Remove(applicationPath.Length - 1, 1);

                    siteDomain = string.Format("{0}{1}", hostName, applicationPath);
                }

                return siteDomain;
            }
        }

        public HttpContext Context
        {
            get;
            private set;
        }

        public User User
        {
            get
            {
                User user = null;
                var userInfo = Context.User.Identity.Name;

                if (!string.IsNullOrEmpty(userInfo))
                {
                    var strId = userInfo.Split('-')[0];
                    var id = 0;
                    int.TryParse(strId, out id);

                    using(var db = new FloraShopContext())
                    {
                        user = db.Users.Include(a => a.UserPoints).FirstOrDefault(a=>a.Id == id);
                    }
                }

                user = user ?? new User() { Username = "Anonymous" };

                return user;
            }
        }

        public bool IsWebRequest
        {
            get { return Context != null; }
        }

        public bool IsAjaxRequest
        {
            get
            {
                return Context != null &&
                       string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"],
                           StringComparison.OrdinalIgnoreCase);
            }
        }

        public RouteData RouteData
        {
            get
            {
                return Context.Request.RequestContext.RouteData;
            }
        }

        public string UserIp
        {
            get
            {
                return Context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public string ServerIp
        {
            get
            {
                var localIps = Dns.GetHostAddresses(Dns.GetHostName());
                var ip = localIps != null ? localIps.FirstOrDefault(a => a.GetAddressBytes().Length == 4) : null;

                return ip != null ? ip.ToString() : "::1";
            }
        }

        public HttpBrowserCapabilities HttpBrowserCapabilities
        {
            get { return Context.Request.Browser; }
        }

        public string BrowserDetectionInfo
        {
            get
            {
                var browserDetectionInfo = string.Empty;

                if (Current.HttpBrowserCapabilities == null)
                {
                    return "js";
                }

                var userAgent = Current.HttpBrowserCapabilities.Browser.ToLower();
                var platform = Current.HttpBrowserCapabilities.Platform.ToLower();
                var version = Current.HttpBrowserCapabilities.Version.ToLower();
                var mobileDevice = Current.HttpBrowserCapabilities.MobileDeviceModel.ToLower();

                if (Regex.IsMatch(userAgent, @"ie") || Regex.IsMatch(userAgent, @"internetexplorer"))
                {
                    browserDetectionInfo = "ie ie" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"firefox"))
                {
                    browserDetectionInfo = "ff ff" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"gecko"))
                {
                    browserDetectionInfo = "gecko";
                }
                else if (Regex.IsMatch(userAgent, @"opera"))
                {
                    browserDetectionInfo = "opera opera" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"konqueror"))
                {
                    browserDetectionInfo = "konqueror";
                }
                else if (Regex.IsMatch(userAgent, @"chrome"))
                {
                    browserDetectionInfo = "chrome chrome" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"safari"))
                {
                    if (Regex.IsMatch(platform, @"unknown"))
                    {
                        browserDetectionInfo = "safari";
                    }
                    else
                    {
                        browserDetectionInfo = "safari safari" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                    }

                }
                else if (Regex.IsMatch(userAgent, @"mozilla"))
                {
                    browserDetectionInfo = "gecko";
                    if (Regex.IsMatch(Current.HttpBrowserCapabilities.Id, @"webkit"))
                    {
                        browserDetectionInfo = "safari safari" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                    }
                }

                if (Regex.IsMatch(platform, @"j2me"))
                {
                    browserDetectionInfo += " mobile";
                }
                else if (Regex.IsMatch(platform, @"darwin"))
                {
                    browserDetectionInfo += " mac";
                }
                else if (Regex.IsMatch(platform, @"x11"))
                {
                    browserDetectionInfo += " linux";
                }
                else if (Regex.IsMatch(platform, @"winnt"))
                {
                    browserDetectionInfo += " win";
                }
                else if (Regex.IsMatch(platform, @"unknown"))
                {
                    browserDetectionInfo += string.Format(" {0}", mobileDevice);
                }

                else
                {
                    browserDetectionInfo += " " + platform.ToLower();
                }

                browserDetectionInfo += " js";
                return browserDetectionInfo;
            }
        }

        /// <summary>
        /// Gets the claw configuration.
        /// </summary>
        /// <value>
        /// The claw configuration.
        /// </value>
        public SiteConfiguration SiteConfig
        {
            get
            {
                return SiteConfiguration.GetConfig();
            }
        }

        /// <summary>
        /// Gets default return URL query key.
        /// </summary>
        /// <value>
        /// Default return URL query key.
        /// </value>
        public string ReturnUrlQueryKey
        {
            get
            {
                return ConfigurationManager.AppSettings["aspnet:FormsAuthReturnUrlVar"] ?? "returnUrl";
            }
        }

        /// <summary>
        /// Gets a value indicating whether [is mobile device].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is mobile device]; otherwise, <c>false</c>.
        /// </value>
        public bool IsMobileDevice
        {
            get
            {
                if (IsWebRequest && Context.Request != null)
                    return Context.Request.Browser.IsMobileDevice;

                return false;
            }
        }

        public IController Controller { get; set; }
    }
}
