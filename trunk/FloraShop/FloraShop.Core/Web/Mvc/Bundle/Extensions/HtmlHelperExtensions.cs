using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using FloraShop.Core.Web.Mvc.Bundle.Configuration;
using FloraShop.Core.Web.Mvc.Bundle;
using FloraShop.Core.Web.Mvc.Bundle.Models;
using System.IO;
using FloraShop.Core.Logs;
using FloraShop.Core;
using FloraShop.Core.Extensions;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        #region CssBundle

        public static MvcHtmlString CssBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad = false)
        {
            return CssBundle(helper, bundlePath, lazyLoad, 1);
        }

        public static MvcHtmlString CssBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad, int order)
        {
            var jsTag = new TagBuilder("link");
            jsTag.MergeAttribute("rel", "stylesheet");
            jsTag.MergeAttribute("type", "text/css");

            if (lazyLoad)
            {
                jsTag.MergeAttribute("href", "#");
                jsTag.MergeAttribute("data-lazyload", "true");
                jsTag.MergeAttribute("data-order", order.ToString());

                return ReferenceBundle(helper, bundlePath, jsTag, "data-href");
            }

            return ReferenceBundle(helper, bundlePath, jsTag, "href");
        }

        public static MvcHtmlString CssBundle(this HtmlHelper helper, string bundlePath, params string[] filePaths)
        {
            return CssBundle(helper, bundlePath, false, filePaths);
        }

        public static MvcHtmlString CssBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad, params string[] filePaths)
        {
            return CssBundle(helper, bundlePath, lazyLoad, 1, filePaths);
        }

        public static MvcHtmlString CssBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad, int order, params string[] filePaths)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);
            if (bundle == null && filePaths != null)
            {
                var cssBundle = new StyleImageBundle(bundlePath, new CssMinify());

                foreach (var filePath in filePaths)
                    cssBundle.Include(filePath);

                BundleTable.Bundles.Add(cssBundle);
            }

            return CssBundle(helper, bundlePath, lazyLoad, order);
        }

        #endregion

        #region JsBundle

        public static MvcHtmlString JsBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad = false)
        {
            return JsBundle(helper, bundlePath, lazyLoad, 1);
        }

        public static MvcHtmlString JsBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad, int lazyOder)
        {
            var jsTag = new TagBuilder("script");
            jsTag.MergeAttribute("type", "text/javascript");

            if (lazyLoad)
            {
                jsTag.MergeAttribute("data-lazyload", "true");
                jsTag.MergeAttribute("data-order", lazyOder.ToString());

                return ReferenceBundle(helper, bundlePath, jsTag, "data-src");
            }

            return ReferenceBundle(helper, bundlePath, jsTag, "src");
        }

        public static MvcHtmlString JsBundle(this HtmlHelper helper, string bundlePath, params string[] filePaths)
        {
            return JsBundle(helper, bundlePath, false, filePaths);
        }

        public static MvcHtmlString JsBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad, params string[] filePaths)
        {
            return JsBundle(helper, bundlePath, lazyLoad, 1, filePaths);
        }

        public static MvcHtmlString JsBundle(this HtmlHelper helper, string bundlePath, bool lazyLoad, int lazyOrder, params string[] filePaths)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);

            if (bundle == null && filePaths != null)
            {
                bundle = new Bundle(bundlePath, new JsMinify());

                foreach (var filePath in filePaths)
                    bundle.Include(filePath);

                BundleTable.Bundles.Add(bundle);
            }

            return JsBundle(helper, bundlePath, lazyLoad, lazyOrder);
        }

        #endregion

        #region private method

        private static MvcHtmlString ReferenceBundle(HtmlHelper helper, string bundlePath, TagBuilder baseTag, string key)
        {
            var bundle = BundleTable.Bundles.GetBundleFor(bundlePath);
            if (bundle == null)
            {
                var exp = new ArgumentException("Invalid Bundle Path", "bundlePath");
                exp.Log();

                return new MvcHtmlString(string.Empty);
            }

            var tagRenderMode = key.IndexOf("href") >= 0 ? TagRenderMode.SelfClosing : TagRenderMode.Normal;

            var httpContext = helper.ViewContext.HttpContext;
            if (!BundleConfigurationManager.Ignore(httpContext))
            {
                baseTag.MergeAttribute(key, BundleTable.Bundles.ResolveBundleUrl(bundlePath));
                return new MvcHtmlString(baseTag.ToString(tagRenderMode));
            }

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var bundleContext = new BundleContext(helper.ViewContext.HttpContext, BundleTable.Bundles, urlHelper.Content(bundlePath));
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

            return new MvcHtmlString(htmlString.ToString());
        }

        #endregion
    }
}