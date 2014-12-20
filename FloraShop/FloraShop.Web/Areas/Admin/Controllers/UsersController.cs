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

namespace FloraShop.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        public UsersController(FloraShopContext db)
            : base(db)
        {

        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = DbContext.Users.Include(u => u.District).Include(u => u.Province);
            return View(users.ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
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
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(DbContext.Districts, "Id", "Name");
            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,PasswordSalt,Email,FullName,Active,IsAdmin,CreatedDate,UpdatedDate,LastLogin,ResetCode,ResetExpiredCode,Cellphone,Telphone,Address,TotalPoints,Birthday,Gender,DistrictId,ProvinceId")] User user)
        {
            if (ModelState.IsValid)
            {
                DbContext.Users.Add(user);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(DbContext.Districts, "Id", "Name", user.DistrictId);
            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", user.ProvinceId);
            return View(user);
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

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,PasswordSalt,Email,FullName,Active,IsAdmin,CreatedDate,UpdatedDate,LastLogin,ResetCode,ResetExpiredCode,Cellphone,Telphone,Address,TotalPoints,Birthday,Gender,DistrictId,ProvinceId")] User user)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(user).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
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
            User user = DbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = DbContext.Users.Find(id);
            DbContext.Users.Remove(user);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
