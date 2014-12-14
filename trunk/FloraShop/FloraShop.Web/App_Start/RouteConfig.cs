using FloraShop.Core.Web.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace FloraShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.RouteExistingFiles = true;

            // new in mvc5
            routes.MapMvcAttributeRoutes();

            RouteTableRegister.RegisterRoutes(routes);

            GlobalConfiguration.Configure(config =>
               {
                   config.MapHttpAttributeRoutes();

                   config.Routes.MapHttpRoute(
                       name: "DefaultApi",
                       routeTemplate: "api/{controller}/{action}/{id}",
                       defaults: new { id = RouteParameter.Optional }
                   );


               });
        }
    }
}
