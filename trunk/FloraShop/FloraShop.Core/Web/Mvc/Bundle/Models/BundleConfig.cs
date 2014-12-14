using System.Collections.Generic;

namespace FloraShop.Core.Web.Mvc.Bundle.Models
{
    public class BundleConfig
    {
        public string BundlePath { get; set; }
        public string CdnPath { get; set; }
        public bool Minify { get; set; }

        public IList<BundleDirectory> Directories { get; set; }
        public IList<BundleFile> Files { get; set; }
    }
}