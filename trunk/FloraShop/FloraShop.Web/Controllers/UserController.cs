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
            return View(new RegisterModel());
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
                    TotalPoints = 0,

                    Username = model.Username,
                    PasswordSalt = salt,
                    Password = pass,

                    FullName = model.FullName,
                    Birthday = model.Birthday,
                    Gender = model.Gender,
                    Email = model.Email
                };

                try
                {
                    DbContext.Users.Add(user);
                    DbContext.SaveChanges();
                }
                catch(Exception exp)
                {
                    exp.Log();
                }
            }

            ViewBag.Error = "Thông tin không hợp lệ. Xin kiểm tra lại.";
            return View(model);
        }

    }
}