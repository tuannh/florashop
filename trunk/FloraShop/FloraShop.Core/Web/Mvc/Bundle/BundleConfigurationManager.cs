using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using FloraShop.Core.Web.Mvc.Bundle.Configuration;
using FloraShop.Core.Web.Mvc.Bundle.Models;
using FloraShop.Core.Configurations;
using System.IO;
using FloraShop.Core.Utility;

namespace FloraShop.Core.Web.Mvc.Bundle
{
    public class BundleConfigurationManager
    {
        private static BundleConfigCollection _collection;

        public static BundleConfigCollection GetBundleConfigCollection()
        {
            if (_collection == null)
                throw new ApplicationException("BundleConfigCollection has not been initialized.");

            return _collection;
        }

        public static void Init(BundleConfigCollection collection = null)
        {
            if (collection == null)
            {
                BundleConfigurationSection config;

                try
                {
                    config = WebConfigurationManager.GetSection("bundleConfig") as BundleConfigurationSection;
                    if (config == null)
                    {
                        // get config from claw config settings
                        var path = Globals.MapPath("~/config/bundles.config");
                        if (File.Exists(path))
                        {
                            config = (BundleConfigurationSection)Activator.CreateInstance(typeof(BundleConfigurationSection));
                            config.DeserializeSection(IOUtility.ReadAsString(path));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Can not find section <bundleConfig> in the configuration file", ex);
                }

                var cssBundles = CreateBundleConfigs(config.CssBundles);
                var jsBundles = CreateBundleConfigs(config.JsBundles);

                collection = new BundleConfigCollection
                {
                    IgnoreIfDebug = config.IgnoreIfDebug,
                    IgnoreIfLocal = config.IgnoreIfLocal,
                    CssBundles = cssBundles,
                    JsBundles = jsBundles
                };
            }

            _collection = collection;
        }

        private static IList<BundleConfig> CreateBundleConfigs(BundleConfigurationElementCollection bundles)
        {
            var result = new List<BundleConfig>();

            foreach (BundleConfigurationElement bundle in bundles)
            {
                var bundleConfig = new BundleConfig
                {
                    BundlePath = bundle.BundlePath,
                    CdnPath = bundle.CdnPath,
                    Minify = bundle.Minify,
                    Directories = new List<BundleDirectory>(),
                    Files = new List<BundleFile>()
                };

                foreach (BundleDirectoryConfigurationElement directory in bundle.Directories)
                {
                    bundleConfig.Directories.Add(new BundleDirectory
                    {
                        DirectoryPath = directory.DirectoryPath,
                        SearchPattern = directory.SearchPattern,
                        SearchSubdirectories = directory.SearchSubdirectories
                    });
                }

                foreach (BundleFileConfigurationElement file in bundle.Files)
                {
                    bundleConfig.Files.Add(new BundleFile
                    {
                        FilePath = file.FilePath
                    });
                }

                result.Add(bundleConfig);
            }

            return result;
        }

        public static bool Ignore(HttpContextBase httpContext)
        {
            var config = GetBundleConfigCollection();

            return (config.IgnoreIfDebug && httpContext.IsDebuggingEnabled)
                   || (config.IgnoreIfLocal && httpContext.Request.IsLocal);
        }
    }
}