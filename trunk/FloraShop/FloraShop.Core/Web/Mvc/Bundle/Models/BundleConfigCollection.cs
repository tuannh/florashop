using System.Collections.Generic;

namespace FloraShop.Core.Web.Mvc.Bundle.Models
{
    public class BundleConfigCollection
    {
        public bool IgnoreIfDebug { get; set; }
        public bool IgnoreIfLocal { get; set; }

        public IList<BundleConfig> CssBundles { get; set; }
        public IList<BundleConfig> JsBundles { get; set; }
    }
}