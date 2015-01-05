using FloraShop.Core.DAL;
using FloraShop.Core.Domain;
using FloraShop.Core;
using FloraShop.Core.Configurations;
using FloraShop.Core.Providers;
using FloraShop.Core.Utility;
using FloraShop.Web.Areas.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FloraShop.Web.Areas.Common.Controllers
{
    public class UserController : Controller
    {
        private FloraShopContext db = new FloraShopContext();

        public string Index()
        {
            RedirectToAction("Login");

            return "";
        }

        public ActionResult Login()
        {
            return View(new UserModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Include(a => a.UserPoints).Where(a => string.Compare(a.Username, model.Username, true) == 0).FirstOrDefault();

                if (user != null && user.IsAdmin)
                {
                    var password = EncryptProvider.EncryptPassword(model.Password, user.PasswordSalt);

                    if (user.Active && user.Password == password)
                    {
                        var userInfo = string.Format("{0}-{1}", user.Id, user.Username);
                        FormsAuthentication.SetAuthCookie(userInfo, model.RememberMe);

                        var key = SiteContext.Current.ReturnUrlQueryKey;
                        var returnUrl = SiteContext.Current.QueryString[key] ?? "";

                        if (string.IsNullOrEmpty(returnUrl))
                            return Redirect(SiteUrls.Instance.DefaultAdminUrl());

                        return Redirect(Globals.ResolveUrl(returnUrl));
                    }
                    else
                    {
                        ViewBag.Error = "Tên đăng nhập/mật khẩu không đúng.";
                        return View(model);
                    }
                }
            }

            ViewBag.Error = "Thông tin không hợp lệ. Xin kiểm tra lại";
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var returnUrl = string.Empty;

            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.ToLower().IndexOf("/logout") < 0)
            {
                returnUrl = Globals.UrlEncode(Request.UrlReferrer.AbsolutePath);
            }

            FormsAuthentication.SignOut();

            var url = SiteUrls.Instance.LoginUrl(returnUrl);

            return Redirect(Globals.ResolveUrl(url));
        }

        public ActionResult ForgotPw()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPw(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = db.Users.Where(a => string.Compare(a.Username, email, true) == 0).FirstOrDefault();
                if (user == null)
                    user = db.Users.Where(a => string.Compare(a.Email, email, true) == 0).FirstOrDefault();

                if (user != null)
                {
                    var newPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                    var salt = EncryptProvider.GenerateSalt();
                    var encryptPass = EncryptProvider.EncryptPassword(newPassword, salt);

                    user.Password = encryptPass;
                    user.PasswordSalt = salt;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    SendEmail(user, newPassword);
                    ViewBag.Message = "Mật khẩu đã được gởi vào email của bạn thành công.";

                    return View();
                }
            }

            ViewBag.Message = "Tên đăng nhập/email không tồn tại.";

            return View();
        }

        private void SendEmail(User user, string newPassword)
        {
            var path = Globals.MapPath("~/Userfiles/Templates/Forgotpw.cshtml");
            if (user != null && System.IO.File.Exists(path))
            {
                var subject = "Thông tin đăng nhập";
                var body = System.IO.File.ReadAllText(path);

                body = body.Replace("{displayname}", user.FullName ?? user.Username);
                body = body.Replace("{username}", user.Username);
                body = body.Replace("{password}", newPassword);

                var settings = SiteConfiguration.GetConfig();

                EmailSender.InstantSend(subject, body, settings.DefaultSender, user.Email);
            }

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