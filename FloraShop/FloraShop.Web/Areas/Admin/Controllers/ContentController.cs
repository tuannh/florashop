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
using FloraShop.Core.Models;
using FloraShop.Core.Controllers;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    public class ContentController : AdminController
    {
        public ContentController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

        // GET: Admin/Content
        public ActionResult Index(string kw)
        {
            List<Content> lst = null;

            if (!string.IsNullOrEmpty(kw))
            {
                var keyword = kw.ToLower().Trim();
                lst = DbContext.Contents.ToList();
                lst = lst.Where(a => a.Name.ToLower().Contains(keyword))
                         .OrderBy(a => a.Name)
                         .ToList();

                if (lst.Count > 0)
                    ViewBag.SearchReseult = string.Format("<b>{0}</b> kết quả được tìm thấy", lst.Count);
                else
                    ViewBag.SearchReseult = string.Format("Không tìm thấy kết quả với từ khóa <b>{0}</b>", kw);
            }
            else
            {
                lst = DbContext.Contents.ToList();
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

        // GET: Admin/Content/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Content/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Alias,Value,PageAlias")] Content content)
        {
            if (ModelState.IsValid)
            {
                DbContext.Contents.Add(content);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(content);
        }

        // GET: Admin/Content/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = DbContext.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Admin/Content/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Alias,Value,PageAlias")] Content content)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(content).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(content);
        }

        // GET: Admin/Content/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = DbContext.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }

            DbContext.Contents.Remove(content);
            DbContext.SaveChanges();
            return RedirectToAction("Index");

            // return View(content);
        }

    }
}
