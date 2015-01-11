using FloraShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FloraShop.Web.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult RobotsTxt()
        {
            var defText = @"
User-agent: *
Disallow: /common/
Disallow: /admin/
";

            var path = Globals.MapPath("~/robots.txt");
            if (System.IO.File.Exists(path))
            {
                defText = System.IO.File.ReadAllText(path);
            }

            return Content(defText, "text/plain");
        }
    }
}