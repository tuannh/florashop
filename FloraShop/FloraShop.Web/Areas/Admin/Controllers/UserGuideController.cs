using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FloraShop.Core.Domain;
using FloraShop.Core.DAL;
using FloraShop.Core.Models;
using FloraShop.Core.Controllers;
using System.IO;
using FloraShop.Core;
using FloraShop.Core.Utility;
using System.Drawing;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    public class UserGuideController : AdminController
    {
        public const string Folder = "~/Userfiles/Upload/images/Modules/userguide/";
        public UserGuideController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }


        // GET: /Admin/UserGuide/
        public ActionResult Index(string kw)
        {
            List<UserGuide> lst = null;

            if (!string.IsNullOrEmpty(kw))
            {
                var keyword = kw.ToLower().Trim();
                lst = DbContext.UserGuides.ToList();
                lst = lst.Where(a => a.Name.ToLower().Contains(keyword) || (a.Content ?? "").ToLower().Contains(keyword))
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
                lst = DbContext.UserGuides.ToList();
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

            // return View(DbContext.UserGuides.ToList());
        }

        // GET: /Admin/UserGuide/Create
        public ActionResult Create()
        {
            return View(new UserGuide());
        }

        // POST: /Admin/UserGuide/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Content,Alias,DisplayOrder,Summary")] UserGuide userguide, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                DbContext.UserGuides.Add(userguide);
                DbContext.SaveChanges();

                if (file != null)
                {
                    var folerPath = Globals.MapPath(Folder);
                    if (!Directory.Exists(folerPath))
                        Directory.CreateDirectory(folerPath);

                    // delete old banner file
                    var path = string.Format("{0}{1}", folerPath, userguide.Photo);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    var filename = string.Format("{0}-{1}", userguide.Id, file.FileName);
                    path = string.Format("{0}{1}", folerPath, filename);

                    var tmpname = string.Format("{0}-{1}", Guid.NewGuid().ToString(), file.FileName);
                    var tmppath = string.Format("{0}{1}", folerPath, tmpname);
                    file.SaveAs(tmppath);

                    var config = FloraShop.Core.Configurations.SiteConfiguration.GetConfig();
                    var bannerConfig = config.UserGuide;
                    ImageTools.FixResizeImage(tmppath, path, bannerConfig.ThumbWidth, bannerConfig.ThumbHeight, ColorTranslator.FromHtml(bannerConfig.Background), config.Quality);

                    try { System.IO.File.Delete(tmppath); }
                    catch { }

                    userguide.Photo = filename;
                    DbContext.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(userguide);
        }

        // GET: /Admin/UserGuide/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGuide userguide = DbContext.UserGuides.Find(id);
            if (userguide == null)
            {
                return HttpNotFound();
            }
            return View(userguide);
        }

        // POST: /Admin/UserGuide/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Content,Alias,DisplayOrder,Summary,Photo")] UserGuide userguide, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var folerPath = Globals.MapPath(Folder);
                    if (!Directory.Exists(folerPath))
                        Directory.CreateDirectory(folerPath);

                    var path = string.Format("{0}{1}", folerPath, userguide.Photo);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    var filename = string.Format("{0}-{1}", userguide.Id, file.FileName);
                    path = string.Format("{0}{1}", folerPath, filename);

                    var tmpname = string.Format("{0}-{1}", Guid.NewGuid().ToString(), file.FileName);
                    var tmppath = string.Format("{0}{1}", folerPath, tmpname);
                    file.SaveAs(tmppath);

                    var config = FloraShop.Core.Configurations.SiteConfiguration.GetConfig();
                    var newsConfig = config.News;
                    ImageTools.FixResizeImage(tmppath, path, newsConfig.ThumbWidth, newsConfig.ThumbHeight, ColorTranslator.FromHtml(newsConfig.Background), config.Quality);

                    try { System.IO.File.Delete(tmppath); }
                    catch { }

                    userguide.Photo = filename;
                }


                DbContext.Entry(userguide).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userguide);
        }

        // GET: /Admin/UserGuide/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGuide userguide = DbContext.UserGuides.Find(id);
            if (userguide == null)
            {
                return HttpNotFound();
            }

            DbContext.UserGuides.Remove(userguide);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
