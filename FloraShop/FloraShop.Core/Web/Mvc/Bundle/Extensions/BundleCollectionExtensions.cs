using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using FloraShop.Core.Web.Mvc.Bundle;
using FloraShop.Core.Web.Mvc.Bundle.Configuration;
using FloraShop.Core.Web.Mvc.Bundle.Models;

namespace System.Web.Optimization
{
    public static class BundleCollectionExtensions
    {
        public static void RegisterConfigurationBundles(this BundleCollection bundles, BundleConfigCollection bundleConfigCollection = null)
        {
            BundleConfigurationManager.Init(bundleConfigCollection);

            var config = BundleConfigurationManager.GetBundleConfigCollection();

            AddBundleConfiguration<CssMinify>(bundles, config.CssBundles);
            AddBundleConfiguration<JsMinify>(bundles, config.JsBundles);
        }

        private static void AddBundleConfiguration<T>(BundleCollection bundles, IEnumerable<BundleConfig> bundleConfigs)
            where T : IBundleTransform, new()
        {
            foreach (var bundleConfig in bundleConfigs)
            {
                var transform = bundleConfig.Minify ? (IBundleTransform)new T() : null;

                Bundle bundle = null;

                if (transform is JsMinify)
                {
                    bundle = String.IsNullOrWhiteSpace(bundleConfig.CdnPath) ?
                                new Bundle(bundleConfig.BundlePath, transform) :
                                new Bundle(bundleConfig.BundlePath, bundleConfig.CdnPath, transform);
                }
                else
                {
                    bundle = String.IsNullOrWhiteSpace(bundleConfig.CdnPath) ?
                                new StyleImageBundle(bundleConfig.BundlePath, transform) :
                                new StyleImageBundle(bundleConfig.BundlePath, bundleConfig.CdnPath, transform);
                }

                foreach (var file in bundleConfig.Files)
                {
                    if (bundle is StyleImageBundle)
                        (bundle as StyleImageBundle).Include(file.FilePath);
                    else
                        bundle.Include(file.FilePath);
                }

                foreach (var directory in bundleConfig.Directories)
                {
                    bundle.IncludeDirectory(directory.DirectoryPath, directory.SearchPattern,
                                            directory.SearchSubdirectories);
                }

                bundles.Add(bundle);
            }
        }
    }
}
