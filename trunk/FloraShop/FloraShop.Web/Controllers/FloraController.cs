using FloraShop.Core.DAL;
using FloraShop.Core;
using FloraShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using FloraShop.Core.Controllers;
using System.Web.Mvc;

namespace FloraShop.Web.Controllers
{
    public class FloraController : FrontController
    {
        public FloraController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

        // POST: api/flora
        [HttpPost]
        public ActionResult ValidUser(string username)
        {
            var db = SiteContext.Current.DbContext;
            var user = db.Users.Where(a => string.Compare(a.Username, username, true) == 0).FirstOrDefault();

            return Json(new { valid = (user != null ? 0 : 1) }); ;
        }

        // POST: api/flora
        [HttpPost]
        [ActionName("validemail")]
        public ActionResult ValidEmail(string email)
        {
            var db = SiteContext.Current.DbContext;
            var user = db.Users.Where(a => string.Compare(a.Email, email, true) == 0).FirstOrDefault();

            return Json(new { valid = (user != null ? 0 : 1) }); ;
        }
    }
}
