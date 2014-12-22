using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FloraShop.Core.DAL;
using FloraShop.Core.Domain;
using FloraShop.Core.Controllers;
using FloraShop.Core.Models;
using FloraShop.Core.Extensions;
using FloraShop.Core.Providers;
using FloraShop.Web.Filters;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class UsersController : AdminController
    {
        public UsersController(FloraShopContext db)
            : base(db)
        {
        }

        // GET: Admin/Users
        public ActionResult Index(string kw)
        {
            List<User> lst = null;

            if (!string.IsNullOrEmpty(kw))
            {
                var keyword = kw.ToLower().Trim();
                lst = DbContext.Users.ToList();
                lst = lst.Where(a => a.Username.ToLower().Contains(keyword) || (a.FullName ?? "").ToLower().Contains(keyword) ||
                                     (a.Email ?? "").ToLower().Contains(keyword))
                         .OrderBy(a => a.Username)
                         .ToList();

                if (lst.Count > 0)
                    ViewBag.SearchReseult = string.Format("<b>{0}</b> kết quả được tìm thấy", lst.Count);
                else
                    ViewBag.SearchReseult = string.Format("Không tìm thấy kết quả với từ khóa <b>{0}</b>", kw);
            }
            else
            {
                lst = DbContext.Users.OrderBy(a => a.Username).ToList();
            }

            var pagingModel = new PagingModel();
            pagingModel.ItemsPerPage = PageSize;
            pagingModel.CurrentPage = PageIndex;
            pagingModel.TotalItems = lst.Count();
            pagingModel.RequestUrl = ControllerContext.RequestContext.HttpContext.Request.RawUrl;

            lst = lst.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.PagingModel = pagingModel;
            ViewBag.Keyword = kw;

            return View(lst);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = DbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.DistrictId = new SelectList(DbContext.Districts, "Id", "Name", user.DistrictId);
            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", user.ProvinceId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Email,FullName,Active,Phone,Address,Gender,DistrictId,ProvinceId")] User user, string userbirthday)
        {
            if (ModelState.IsValid)
            {
                var dbUser = DbContext.Users.Find(user.Id);
                if (dbUser == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // update info
                dbUser.Email = user.Email;
                dbUser.FullName = user.FullName;
                dbUser.Active = user.Active;
                dbUser.Phone = user.Phone;
                dbUser.Address = user.Address;
                dbUser.Gender = user.Gender;
                dbUser.DistrictId = user.DistrictId;
                dbUser.ProvinceId = user.ProvinceId;

                var newPass = EncryptProvider.EncryptPassword(user.Password, dbUser.PasswordSalt);
                dbUser.Password = newPass; 

                var birthday = DateTime.Now.GetDate(userbirthday, "dd/MM/yyyy");
                if (birthday.HasValue)
                    dbUser.Birthday = birthday;

                DbContext.Entry(dbUser).State = EntityState.Modified;
                DbContext.SaveChanges();

                return RedirectToAction("index");
            }

            ViewBag.DistrictId = new SelectList(DbContext.Districts, "Id", "Name", user.DistrictId);
            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", user.ProvinceId);

            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = DbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            DbContext.Users.Remove(user);
            DbContext.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
