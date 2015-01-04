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
using FloraShop.Web.Filters;
using FloraShop.Core.Enumerations;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class OrderController : AdminController
    {
        public OrderController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

        // GET: Admin/Order
        public ActionResult Index(string kw, int? status, int? userid)
        {
            var lst = DbContext.Orders.Include(a => a.User).ToList();

            if (status.HasValue && status.Value > 0)
                lst = lst.Where(o => o.Status == status).ToList();

            if (userid.HasValue && userid.Value > 0)
            {
                lst = lst.Where(u => u.User != null && u.User.Id == userid).ToList();
                var user = DbContext.Users.Find(userid.Value);
                ViewBag.User = user;
            }

            if (!string.IsNullOrEmpty(kw))
            {
                var keyword = kw.ToLower().Trim();
                lst = lst.Where(a => a.CustomerName.ToLower().Contains(keyword) || (a.Address ?? "").ToLower().Contains(keyword) || (a.Phone ?? "").ToLower().Contains(keyword))
                         .OrderByDescending(a => a.OrderDate)
                         .ThenBy(a => a.CustomerName)
                         .ToList();

                if (lst.Count > 0)
                    ViewBag.SearchReseult = string.Format("<b>{0}</b> kết quả được tìm thấy", lst.Count);
                else
                    ViewBag.SearchReseult = string.Format("Không tìm thấy kết quả với từ khóa <b>{0}</b>", kw);
            }
            else
            {
                lst = lst.OrderByDescending(a => a.OrderDate)
                         .ThenBy(a => a.CustomerName)
                         .ToList();
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = DbContext.Orders.Include(a => a.ProductOrders.Select(b => b.Product))
                                         .Where(c => c.Id == id)
                                         .FirstOrDefault();

            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", order.ProvinceId);
            ViewBag.DistrictId = new SelectList(DbContext.Districts.Where(a => a.ProvinceId == order.ProvinceId).ToList(), "Id", "Name", order.DistrictId);

            return View(order);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,CustomerName,Phone,Email,Address,DistrictId,Note,OrderDate,ProvinceId,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(order).State = EntityState.Modified;
                DbContext.SaveChanges();

                if (order.Status == (int)OrderStatus.Delivery && order.UserId.HasValue)
                {
                    var total = 0.0;
                    var userpoint = DbContext.UserPoints.Where(a => a.UserId == order.UserId && a.OrderId == order.Id).FirstOrDefault();
                    if (userpoint == null)
                    {
                        userpoint = new UserPoint()
                        {
                            UserId = order.UserId.Value,
                            OrderId = order.Id,
                            Points = (int)total / 100,
                            Note = string.Format("Cộng điểm cho đơn hàng #{0}", order.Id)
                        };

                        DbContext.UserPoints.Add(userpoint);
                        DbContext.SaveChanges();
                    }

                }
                return RedirectToAction("index");
            }

            ViewBag.ProvinceId = new SelectList(DbContext.Provinces, "Id", "Name", order.ProvinceId);
            ViewBag.DistrictId = new SelectList(DbContext.Districts.Where(a=> a.ProvinceId == order.ProvinceId).ToList() , "Id", "Name", order.DistrictId);

            return View(order);
        }


        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = DbContext.Orders.Include(a => a.ProductOrders.Select(b => b.Product))
                                         .Where(c => c.Id == id)
                                         .FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = DbContext.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            DbContext.Orders.Remove(order);
            DbContext.SaveChanges();

            return RedirectToAction("index");

            // return View(order);
        }

    }
}
