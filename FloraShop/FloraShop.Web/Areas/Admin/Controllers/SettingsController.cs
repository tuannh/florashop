using FloraShop.Core.Configurations;
using FloraShop.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class SettingsController : Controller
    {
        //
        // GET: /Admin/Settings/
        public ActionResult Index()
        {
          

            return View();
        }
	}
}