using FloraShop.Core.DAL;
using FloraShop.Core;
using FloraShop.Core.Providers;
using FloraShop.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FloraShop.Web.Filters;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class UserController : Controller
    {
        private FloraShopContext db = new FloraShopContext();

        public ActionResult ChangePwd()
        {
            var model = new ChangePassModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePwd(ChangePassModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(SiteContext.Current.User.Id);
                var password = EncryptProvider.EncryptPassword(model.OldPassword, user.PasswordSalt);

                if (user.Password == password)
                {
                    var newPass = EncryptProvider.EncryptPassword(model.NewPassword, user.PasswordSalt);
                    user.Password = newPass;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Message = "Đổi mật khẩu thành công.";

                    return View(new ChangePassModel());
                }

                ViewBag.Message = "Mật khẩu cũ chưa đúng";

            }

            return View(new ChangePassModel());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}