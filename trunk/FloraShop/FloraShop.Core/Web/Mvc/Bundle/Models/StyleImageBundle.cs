using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using FloraShop.Core.Web.Mvc.Bundle;
using System.IO;
using FloraShop.Core.Configurations;
using FloraShop.Core.Extensions;

namespace System.Web.Optimization
{
    public class StyleImageBundle : Bundle
    {
        public StyleImageBundle(string virtualPath, IBundleTransform fransform)
            : base(virtualPath, fransform)
        {
        }

        public StyleImageBundle(string virtualPath, string cdnPath, IBundleTransform fransform)
            : base(virtualPath, cdnPath, fransform)
        {
        }

        public new Bundle Include(params string[] virtualPaths)
        {
            var config = BundleConfigurationManager.GetBundleConfigCollection();

            // if (HttpContext.Current.IsDebuggingEnabled)
            if (config.IgnoreIfLocal)
            {
                // Debugging. Bundling will not occur so act normal and no one gets hurt.
                base.Include(virtualPaths.ToArray());
                return this;
            }

            // In production mode so CSS will be bundled. Correct image paths.
            var bundlePaths = new List<string>();
            var svr = HttpContext.Current.Server;
            foreach (var path in virtualPaths)
            {
                if (!File.Exists(svr.MapPath(path)))
                    continue;

                var pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
                var contents = IO.File.ReadAllText(svr.MapPath(path));
                var matches = pattern.Matches(contents);

                if (matches == null || matches.Count == 0)
                {
                    bundlePaths.Add(path);
                    continue;
                }

                var bundlePath = (IO.Path.GetDirectoryName(path) ?? string.Empty).Replace(@"\", "/") + "/";
                var bundleUrlPath = VirtualPathUtility.ToAbsolute(bundlePath);
                var bundleFilePath = String.Format("{0}{1}{2}",
                                                   bundlePath,
                                                   IO.Path.GetFileNameWithoutExtension(path),
                                                   IO.Path.GetExtension(path));

                var bundleParentUrlPath = (IO.Path.GetDirectoryName(bundleUrlPath.TrimEnd('/')) ?? "").Replace(@"\", "/") + "/";
                var updateImageUrl = false;

                foreach (Match match in matches)
                {
                    var url = match.Groups.Count > 2 ? match.Groups[2].Value : "";
                    if (string.IsNullOrEmpty(url) || url.StartsWith("/"))
                        continue;

                    if (url.StartsWith("../"))
                    {
                        var newValue = string.Format("url('{0}{1}')", bundleParentUrlPath, url.Replace("../", ""));
                        contents = contents.Replace(match.Value, newValue.ToLower());
                    }
                    else
                    {
                        var newValue = string.Format("url('{0}{1}')", bundleUrlPath, url);
                        contents = contents.Replace(match.Value, newValue.ToLower());
                    }

                    updateImageUrl = true;
                }

                if (!updateImageUrl)
                {
                    bundlePaths.Add(path);
                    continue;
                }

                var tmpPath = svr.MapPath(bundleFilePath);
                var info = new FileInfo(tmpPath);

                if (info.Exists)
                {
                    var bkPath = string.Format("{0}.original.bak", tmpPath);
                    var bkInfo = new FileInfo(bkPath);
                    try
                    {
                        if (bkInfo.Exists)
                            bkInfo.Delete();

                        File.Move(tmpPath, bkPath);
                    }
                    catch (Exception exp)
                    {
                        exp.Log();
                    }

                    IO.File.WriteAllText(tmpPath, contents);
                }
                else if (!info.Exists || !info.IsReadOnly)
                {
                    IO.File.WriteAllText(tmpPath, contents);
                }

                bundlePaths.Add(bundleFilePath);
            }

            base.Include(bundlePaths.ToArray());

            return this;
        }
    }
}
