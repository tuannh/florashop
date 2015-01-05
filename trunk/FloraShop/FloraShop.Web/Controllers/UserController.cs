using FloraShop.Core;
using FloraShop.Core.Controllers;
using FloraShop.Core.DAL;
using FloraShop.Core.Providers;
using FloraShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DM = FloraShop.Core.Domain;
using FloraShop.Core.Extensions;
using System.Threading;
using System.Globalization;
using System.Data.Entity;
using FloraShop.Core.Domain;
using FloraShop.Core.Utility;
using FloraShop.Core.Configurations;
using FloraShop.Web.Filters;

namespace FloraShop.Web.Controllers
{

    public class UserController : FrontController
    {
        public UserController(FloraShopContext dbContext)
            : base(dbContext)
        {
        }

        // GET: User
        public ActionResult Index()
        {
            return Redirect("/san-pham/");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password,RememberMe")] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.Include(a => a.UserPoints).Where(a => string.Compare(a.Username, model.Username, true) == 0).FirstOrDefault();

                if (user != null)
                {
                    var password = EncryptProvider.EncryptPassword(model.Password, user.PasswordSalt);

                    if (user.Active && user.Password == password)
                    {
                        var userInfo = string.Format("{0}-{1}", user.Id, user.Username);
                        FormsAuthentication.SetAuthCookie(userInfo, model.RememberMe);

                        var key = SiteContext.Current.ReturnUrlQueryKey;
                        var returnUrl = SiteContext.Current.QueryString[key] ?? "";

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(SiteUrls.Instance.DefaultAdminUrl());

                        return Redirect("/san-pham/");
                    }
                }
            }

            return Redirect("/dang-nhap/");
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

            return Redirect("/san-pham/");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var salt = EncryptProvider.GenerateSalt();
                var pass = EncryptProvider.EncryptPassword(model.Password, salt);

                var user = new DM.User()
                {
                    Active = true,
                    IsAdmin = false,
                    CreatedDate = DateTime.Now,

                    Username = model.Username,
                    PasswordSalt = salt,
                    Password = pass,

                    FullName = model.FullName,
                    Birthday = DateTime.Now.GetDate(model.Birthday, "dd/MM/yyyy"),
                    Gender = model.Gender.HasValue ? model.Gender.Value : 0,
                    Email = model.Email
                };

                try
                {
                    DbContext.Users.Add(user);
                    DbContext.SaveChanges();

                    ViewBag.Error = 0;
                    ViewBag.Message = "Đăng ký tài khoản thành công.";

                    ModelState.Clear();

                    // return RedirectToAction("register");
                    return View();
                }
                catch (Exception exp)
                {
                    exp.Log();
                }
            }

            ViewBag.Error = 1;
            ViewBag.Message = "Thông tin không hợp lệ. Xin kiểm tra lại.";
            return View(model);
        }

        public ActionResult Edit()
        {
            var user = SiteContext.Current.User;
            var model = new UserModel(user);

            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", user.ProvinceId);
            ViewBag.DistrictId = new SelectList(DbContext.Districts, "Id", "Name", user.DistrictId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var id = SiteContext.Current.User.Id;
                var user = DbContext.Users.Find(id);


                if (user == null)
                    return Redirect("/dang-nhap/");

                model.UpdateDomain(ref user);
                DbContext.Entry(user).State = EntityState.Modified;
                DbContext.SaveChanges();

                ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", user.ProvinceId);
                ViewBag.DistrictId = new SelectList(DbContext.Districts, "Id", "Name", user.DistrictId);

                ViewBag.Message = "Cập nhật thông tin thành công.";
            }

            return View();

        }

        public ActionResult Orders()
        {
            var id = SiteContext.Current.User.Id;
            var user = DbContext.Users.Include(a => a.Orders).Where(a => a.Id == id).FirstOrDefault();
            var model = user.Orders.ToList();

            return View(model);
        }

        public ActionResult OrderDetail(int id)
        {
            var model = DbContext.Orders.Include(a => a.ProductOrders.Select(b => b.Product)).Where(a => a.Id == id).First();
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        public ActionResult ForgotPw()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPw(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var user = DbContext.Users.Where(a => string.Compare(a.Username, username, true) == 0).FirstOrDefault();
                if (user == null)
                    user = DbContext.Users.Where(a => string.Compare(a.Email, username, true) == 0).FirstOrDefault();

                if (user != null)
                {
                    var newPassword = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var salt = EncryptProvider.GenerateSalt();
                    var encryptPass = EncryptProvider.EncryptPassword(newPassword, salt);

                    user.Password = encryptPass;
                    user.PasswordSalt = salt;

                    DbContext.Entry(user).State = EntityState.Modified;
                    DbContext.SaveChanges();

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

        public ActionResult Points()
        {
            var id = SiteContext.Current.User.Id;
            var user = DbContext.Users.Include(a => a.UserPoints).Where(a => a.Id == id).FirstOrDefault();
            var model = user.UserPoints.ToList();

            return View(model);
        }
    }
}