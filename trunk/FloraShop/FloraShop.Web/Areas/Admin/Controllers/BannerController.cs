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
using FloraShop.Core;
using FloraShop.Core.Extensions;
using System.IO;
using FloraShop.Core.Utility;
using System.Drawing;
using System.Collections;
using FloraShop.Web.Filters;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class BannerSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Quality { get; set; }

        public Color BgColor { get; set; }
    }

    public class BannerController : AdminController
    {
        public const string Folder = "~/Userfiles/Modules/BannerRotators/";
        public BannerSettings HomeSettings { get; private set; }
        public BannerSettings ContentSettings { get; private set; }

        public BannerController(FloraShopContext dbContext)
            : base(dbContext)
        {
            HomeSettings = new BannerSettings() { Width = 1800, Height = 1186, BgColor = Color.White, Quality = 80 };
            ContentSettings = new BannerSettings() { Width = 1366, Height = 420, BgColor = Color.White, Quality = 80 };
        }

        // GET: Admin/Banner
        public ActionResult Index(string kw)
        {
            List<Banner> lst = null;

            if (!string.IsNullOrEmpty(kw))
            {
                var keyword = kw.ToLower().Trim();
                lst = DbContext.Banners.ToList();
                lst = lst.Where(a => a.Name.ToLower().Contains(keyword) || (a.Description ?? "").ToLower().Contains(keyword) ||
                                     (a.Url ?? "").ToLower().Contains(keyword) || (a.Target ?? "").ToLower().Contains(keyword))
                         .OrderBy(a => a.DisplayOrder)
                         .ThenBy(a => a.Name)
                         .ToList();

                if (lst.Count > 0)
                    ViewBag.SearchReseult = string.Format("<b>{0}</b> kết quả được tìm thấy", lst.Count);
                else
                    ViewBag.SearchReseult = string.Format("Không tìm thấy kết quả với từ khóa <b>{0}</b>", kw);
            }
            else
            {
                lst = DbContext.Banners.OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name).ToList();
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

        // GET: Admin/Banner/Create
        public ActionResult Create()
        {
            return View(new Banner());
        }

        // POST: Admin/Banner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Order,FileName,Active,Target,Url,DisplayOrder,Category")] Banner banner, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                DbContext.Banners.Add(banner);
                DbContext.SaveChanges();

                if (file != null)
                {
                    var folerPath = Globals.MapPath(Folder);
                    if (!Directory.Exists(folerPath))
                        Directory.CreateDirectory(folerPath);

                    // delete old banner file
                    var path = string.Format("{0}{1}", folerPath, banner.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    var filename = string.Format("{0}-{1}", banner.Id, file.FileName);
                    path = string.Format("{0}{1}", folerPath, filename);

                    try
                    {
                        var tmpname = string.Format("{0}-{1}", Guid.NewGuid().ToString(), file.FileName);
                        var tmppath = string.Format("{0}{1}", folerPath, tmpname);
                        file.SaveAs(tmppath);

                        var settings = banner.Category == 0 ? HomeSettings : ContentSettings;
                        ImageTools.FixResizeImage(tmppath, path, settings.Width, settings.Height, settings.BgColor, settings.Quality);

                        System.IO.File.Delete(tmppath);
                    }
                    catch (Exception exp)
                    {
                        exp.Log();
                    }

                    banner.FileName = filename;
                    DbContext.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Admin/Banner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = DbContext.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,FileName,Active,Target,Url,DisplayOrder,Category")] Banner banner, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    var folerPath = Globals.MapPath(Folder);
                    if (!Directory.Exists(folerPath))
                        Directory.CreateDirectory(folerPath);

                    var path = string.Format("{0}{1}", folerPath, banner.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    var filename = string.Format("{0}-{1}", banner.Id, file.FileName);
                    path = string.Format("{0}{1}", folerPath, filename);

                    try
                    {
                        var tmpname = string.Format("{0}-{1}", Guid.NewGuid().ToString(), file.FileName);
                        var tmppath = string.Format("{0}{1}", folerPath, tmpname);
                        file.SaveAs(tmppath);

                        var settings = banner.Category == 0 ? HomeSettings : ContentSettings;
                        ImageTools.FixResizeImage(tmppath, path, settings.Width, settings.Height, settings.BgColor, settings.Quality);

                        System.IO.File.Delete(tmppath);
                    }
                    catch (Exception exp)
                    {
                        exp.Log();
                    }

                    banner.FileName = filename;
                }

                DbContext.Entry(banner).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("index");
            }
            return View(banner);
        }

        // GET: Admin/Banner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = DbContext.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }

            var filePath = Globals.MapPath(Folder) + banner.FileName;

            DbContext.Banners.Remove(banner);
            DbContext.SaveChanges();

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch { }
            }

            return RedirectToAction("index");
        }

    }
}
