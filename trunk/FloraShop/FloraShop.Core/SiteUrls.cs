using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using FloraShop.Core.Domain;

namespace FloraShop.Core
{
    /// <summary>
    /// Developer: TuanNH
    /// Description: Site Urls utitlity class
    /// Created date: 2013.09.11
    /// </summary>
    public class SiteUrls
    {
        static SiteUrls siteUrls;
        public static SiteUrls Instance
        {
            get
            {
                siteUrls = siteUrls ?? new SiteUrls();

                return siteUrls;
            }
        }

        #region common/error link

        public string LoginUrl(string returnUrl = "")
        {
            string url = "/common/user/login";
            Route route = RouteTable.Routes["Common_Login"] as Route;

            if (route != null)
                url = route.Url.ToLower();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                url = Globals.AppendQueryStringValue(url, SiteContext.Current.ReturnUrlQueryKey, returnUrl);
            }

            return Globals.ResolveClientUrl(url.ToLower());
        }

        public string LogoutUrl()
        {
            return GetUrl("Common_Logout", string.Empty, string.Empty);
        }

        public string ForgotPasswordUrl()
        {
            return GetUrl("Common_ForgotPassword", string.Empty, string.Empty);
        }

        public string ResetPasswordUrl(string token)
        {
            return GetUrl("Common_ResetPassword", string.Empty, string.Empty, new { token = token });
        }

        public string CommonUrl(string controller, string action = "Index", string id = "")
        {
            return GetUrl("Common_Default", controller, action, new { id = id });
        }

        #endregion

        #region admin link

        public string DefaultAdminUrl()
        {
            return AdminUrl("Dashboard");
        }

        public string AdminUrl(string controller, string action = "Index", string id = "")
        {
            return GetUrl("Admin_Default", controller, action, new { id = id });
        }

        #endregion

        #region frontend link

        public string DefaultFrontUrl()
        {
            return "/";
        }

        /// <summary>
        /// Fronts the end URL.
        /// </summary>
        /// <param name="lang">The language code with culture.</param>
        /// <param name="frontEndPage">The front end page.</param>
        /// <returns></returns>
        public string FrontEndUrl(string frontEndPage)
        {
            var ctx = SiteContext.Current;

            var url = GetUrl("FrontEnd_DefaultPage", "Page", "Index", new { FrontEndPage = frontEndPage });

            if (ctx.SiteConfig.AddTrailingSlash && !url.EndsWith("/"))
                url = string.Format("{0}/", url);

            return url;
        }

        /// <summary>
        /// Get previous of current page. Return "#" if previous page is null
        /// </summary>
        /// <returns></returns>
        public string PreviousUrl()
        {
            if (HttpContext.Current.Request != null &&
                HttpContext.Current.Request.UrlReferrer != null)
            {
                return HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            }

            return "#";
        }


        #endregion

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="routeName">Name of the route. This value is defined in routes.config</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="keyPairValues">The key pair values. Ex: new {id=10, lang="en-Us"} or routeValueDictionary object</param>
        /// <returns></returns>
        public string GetUrl(string routeName, string controller, string action, object keyPairValues = null)
        {
            RouteValueDictionary parameters = null;

            if (keyPairValues is RouteValueDictionary)
                parameters = keyPairValues as RouteValueDictionary;
            else if (keyPairValues != null)
                parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(keyPairValues);

            if (parameters == null)
                parameters = new RouteValueDictionary();

            parameters.Add("controller", controller);
            parameters.Add("action", action);

            var vpd = RouteTable.Routes.GetVirtualPath(null, routeName, parameters);
            if (vpd != null)
                return vpd.VirtualPath.ToLower();
            else
            {
                if (!string.IsNullOrEmpty(routeName))
                {
                    Route route = RouteTable.Routes[routeName] as Route;
                    if (route != null)
                    {
                        string url = route.Url.ToLower();
                        StringBuilder key = null;

                        foreach (KeyValuePair<string, object> kp in parameters)
                        {
                            key = new StringBuilder();
                            key.Append("{");
                            key.Append(kp.Key.ToLower());
                            key.Append("}");

                            url = url.Replace(key.ToString(), kp.Value != null ? kp.Value.ToString() : string.Empty);
                        }

                        return Globals.ResolveClientUrl(url);
                    }
                }
            }

            return "/";
        }

        #region page/module link

        /// <summary>
        /// Pages the component settings URL.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="pageComponentId">The page component unique identifier.</param>
        /// <returns></returns>
        public string PageComponentSettingsUrl(string controller, int pageComponentId)
        {
            return GetUrl("Page_PageCompSetting", controller, "ComponentSettings", new { pagecomponentid = pageComponentId });
        }

        /// <summary>
        /// Page component designs the URL.
        /// </summary>
        /// <param name="pageAlias">The page alias.</param>
        /// <returns></returns>
        public string DesignUrl(string pageAlias)
        {
            return GetUrl("Page_ComponentDesign", "Page", "Index", new { frontendpage = pageAlias });
        }
        #endregion

        #region product

        public string GetProduct(Product product)
        {
            return string.Format("/chi-tiet-sp/{0}", product.Alias);
        }

        #endregion
    }
}
