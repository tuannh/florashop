using System;
using System.Web;
using System.Web.Routing;

namespace FloraShop.Core.Web.Mvc.Routing
{
    /// <summary>
    /// Redirect Route Handler
    /// </summary>
    public class RedirectRouteHandler : IRouteHandler
    {
        private readonly string _url;

        public RedirectRouteHandler(string url)
        {
            _url = url;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new RedirectHandler(_url);
        }
    }

    /// <summary>
    /// <para>Redirecting MVC handler</para>
    /// </summary>
    public class RedirectHandler : IHttpHandler
    {
        private readonly string _url;

        public RedirectHandler(string url)
        {
            _url = url;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            httpContext.Response.Status = "301 Moved Permanently";
            httpContext.Response.StatusCode = 301;
            httpContext.Response.AppendHeader("Location", _url);
            return;
        }
    }
}
