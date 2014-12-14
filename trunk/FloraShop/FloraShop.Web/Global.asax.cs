using FloraShop.Core.DAL;
using FloraShop.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using WebApp;
using Ninject;
using FloraShop.Core;

namespace FloraShop.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

#if  DEBUG
            var dbContext = IoC.Kernel.Get<FloraShopContext>();
            dbContext.Initialize();
#endif

            SiteConfiguration.GetConfig();

            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.RegisterConfigurationBundles();
            // BundleConfig.RegisterBundles(BundleTable.Bundles);

            // force using Razor view engine with cshtml extension.
            ViewEngines.Engines.Clear();
            var razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(razorEngine);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Ensure SessionID in order to prevent the folloing exception
            //when the Application Pool Recycles
            //[HttpException]: Session state has created a session id, but cannot
            //    save it because the response was already flushed by 
            string sessionId = Session.SessionID;
        }

        #region api session required

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        public static string _WebApiExecutionPath = "/api/";

        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains(_WebApiExecutionPath);
        }

        #endregion
    }
}
