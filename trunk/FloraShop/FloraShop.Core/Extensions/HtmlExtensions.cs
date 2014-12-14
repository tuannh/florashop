using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FloraShop.Core.BaseObjects;
using FloraShop.Core.Configurations;
using FloraShop.Core.Models;
using FloraShop.Core.Utility;

namespace FloraShop.Core.Extensions
{
    public static class HtmlExtensions
    {
        public static SiteControl Controls(this HtmlHelper html)
        {
            SiteControl.Instance.SetHtmlHelper(html);
            return SiteControl.Instance;
        }

        public static SiteUrls SiteUrls(this HtmlHelper html)
        {
            return FloraShop.Core.SiteUrls.Instance;
        }
    }
}
