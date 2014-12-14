using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FloraShop.Core.Extensions;

namespace FloraShop.Core.Web.Mvc.Bundle
{
    public class BundleManager
    {
        public static string CssBundle(RequestContext requestContext, string bundlePath, params string[] filePaths)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);
            if (bundle == null && filePaths != null)
            {
                StyleImageBundle cssBundle = new StyleImageBundle(bundlePath, new CssMinify());

                foreach (string filePath in filePaths)
                    cssBundle.Include(filePath);

                BundleTable.Bundles.Add(cssBundle);
            }

            return CssBundle(requestContext, bundlePath);
        }

        public static string CssBundle(RequestContext requestContext, string bundlePath)
        {
            var jsTag = new TagBuilder("link");
            jsTag.MergeAttribute("rel", "stylesheet");
            jsTag.MergeAttribute("type", "text/css");

            return ReferenceBundle(requestContext, bundlePath, jsTag, "href");
        }

        public static string ReferenceBundle(RequestContext requestContext, string bundlePath, TagBuilder baseTag, string key)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);
            if (bundle == null)
            {
                var exp = new ArgumentException("Invalid Bundle Path", "bundlePath");
                exp.Log();

                return "";
            }

            TagRenderMode tagRenderMode = key == "href" ? TagRenderMode.SelfClosing : TagRenderMode.Normal;

            var httpContext = requestContext.HttpContext;
            if (!BundleConfigurationManager.Ignore(httpContext))
            {
                baseTag.MergeAttribute(key, BundleTable.Bundles.ResolveBundleUrl(bundlePath));
                return baseTag.ToString(tagRenderMode);
            }

            var urlHelper = new UrlHelper(requestContext);
            var bundleContext = new BundleContext(httpContext, BundleTable.Bundles, urlHelper.Content(bundlePath));
            var htmlString = new StringBuilder();

            foreach (var file in bundle.EnumerateFiles(bundleContext))
            {
                var basePath = httpContext.Server.MapPath("~/");
                var filePath = httpContext.Server.MapPath(file.IncludedVirtualPath);
                if (!filePath.StartsWith(basePath))
                    continue;

                var relPath = urlHelper.Content("~/" + filePath.Substring(basePath.Length));
                baseTag.MergeAttribute(key, relPath, true);
                htmlString.AppendLine(baseTag.ToString(tagRenderMode));
            }

            return htmlString.ToString();
        }

        public static string JsBundle(RequestContext requestContext, string bundlePath, bool lazyLoad = false)
        {
            var jsTag = new TagBuilder("script");
            jsTag.MergeAttribute("type", "text/javascript");

            if (lazyLoad)
            {
                jsTag.MergeAttribute("data-lazyload", "true");

                return ReferenceBundle(requestContext, bundlePath, jsTag, "data-src");
            }

            return ReferenceBundle(requestContext, bundlePath, jsTag, "src");
        }

        public static string JsBundle(RequestContext requestContext, string bundlePath, params string[] filePaths)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);

            if (bundle == null && filePaths != null)
            {
                bundle = new System.Web.Optimization.Bundle(bundlePath, new JsMinify());

                foreach (string filePath in filePaths)
                    bundle.Include(filePath);

                BundleTable.Bundles.Add(bundle);
            }

            return JsBundle(requestContext, bundlePath, false);
        }

        public static string JsBundle(RequestContext requestContext, string bundlePath, bool lazyLoad, params string[] filePaths)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);

            if (bundle == null && filePaths != null)
            {
                bundle = new System.Web.Optimization.Bundle(bundlePath, new JsMinify());

                foreach (string filePath in filePaths)
                    bundle.Include(filePath);

                BundleTable.Bundles.Add(bundle);
            }

            return JsBundle(requestContext, bundlePath, lazyLoad);
        }
    }
}
