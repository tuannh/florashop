using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FloraShop.Core.Extensions;
using FloraShop.Core.DAL;
using FloraShop.Core.Domain;
using FloraShop.Core.Controllers;
using FloraShop.Core.Enumerations;
using FloraShop.Web.Models;
using FloraShop.Core.Configurations;
using FloraShop.Core;
using System.Globalization;
using System.Text;
using FloraShop.Core.Utility;
using FloraShop.Core.Providers;
using System.Web.Security;

namespace FloraShop.Web.Controllers
{
    public class HomeController : FrontController
    {
        public HomeController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

        public ActionResult Index()
        {
            var alias = ControllerContext.RouteData.Values["frontendpage"] != null ? ControllerContext.RouteData.Values["frontendpage"].ToString() : "index";
            var page = DbContext.Pages
                                .Where(p => string.Compare(p.Alias, alias, true) == 0)
                                .FirstOrDefault();

            ViewBag.PageAlias = alias;


            if (page != null)
                return View(page);

            if (alias == "index")
            {
                page = DbContext.Pages.Where(p => p.IsDefault)
                                      .FirstOrDefault();

                if (page != null)
                    return View(page);
            }

            return View();
        }

        public ActionResult PageContent()
        {
            var dbContext = new FloraShopContext();
            var alias = ControllerContext.RouteData.Values["frontendpage"].ToString() ?? "index";
            var page = dbContext.Pages
                                .Where(p => string.Compare(p.Alias, alias, true) == 0)
                                .FirstOrDefault();

            ViewBag.PageAlias = alias;


            if (page != null)
            {
                var pageType = PageType.None;
                Enum.TryParse<PageType>(page.UniqueKey, true, out pageType);
                ViewBag.PageType = pageType;

                if (pageType == PageType.Order)
                {
                    var provinces = DbContext.Provinces.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name }).ToList();
                    provinces.Insert(0, new SelectListItem() { Value = "0", Text = "Chọn tỉnh/thành phố" });


                    var order = new Order();
                    if (Request.IsAuthenticated)
                    {
                        var user = SiteContext.User;

                        order.UserId = user.Id;
                        order.CustomerName = user.FullName;
                        order.Phone = user.Phone;
                        order.Email = user.Email;
                        order.Address = user.Address;
                        order.DistrictId = user.DistrictId;
                        order.ProvinceId = user.ProvinceId;
                    }

                    ViewData["Order"] = order;
                    ViewData["Provinces"] = provinces;
                }
                else if (pageType == PageType.Login)
                {
                    ViewData["LoginModel"] = new LoginModel();
                }

                return View(page);
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Order([Bind(Include = "CustomerName,Phone,Email,Address,DistrictId,ProviceId,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.UserId = order.UserId == 0 ? null : order.UserId;
                order.ProvinceId = order.ProvinceId == 0 ? null : order.ProvinceId;
                order.DistrictId = order.DistrictId == 0 ? null : order.DistrictId;
                order.OrderDate = DateTime.Now;
                order.Status = (int)OrderStatus.New;

                var cartList = Session[MyCart.ShopCart] as List<MyCart>;
                if (cartList != null && cartList.Count > 0)
                {
                    foreach (var cart in cartList)
                    {
                        var productOrder = new ProductOrder()
                        {
                            ProductId = int.Parse(cart.ProductId),
                            Quatity = cart.Quatity,
                            Price = cart.Price
                        };

                        order.ProductOrders.Add(productOrder);
                    }

                    DbContext.Orders.Add(order);
                    DbContext.SaveChanges();

                    var newOrder = DbContext.Orders.Include(a => a.ProductOrders.Select(b => b.Product))
                                            .Where(c => c.Id == order.Id)
                                            .FirstOrDefault();

                    var orderSessionId = Guid.NewGuid().ToString("N");
                    var url = string.Format("/dat-hang/?order={0}", orderSessionId);

                    SendEmail(newOrder);
                    GetOrderInfo(newOrder, orderSessionId);

                    Session[MyCart.ShopCart] = null;

                    return Redirect(url);
                }
            }

            return Redirect("/dat-hang/?order=0");
        }

        protected void SendEmail(Order order)
        {
            var path = Globals.MapPath("~/Userfiles/Templates/Order.cshtml");
            var rowPath = Globals.MapPath("~/Userfiles/Templates/Order-Row.cshtml");

            if (order != null && System.IO.File.Exists(path))
            {
                var bodyTemplate = System.IO.File.ReadAllText(path);
                var rowTemplate = "";

                if (System.IO.File.Exists(rowPath))
                    rowTemplate = System.IO.File.ReadAllText(rowPath);

                var settings = SiteConfiguration.GetConfig();

                try
                {
                    var subject = string.Format("Xác nhận đơn hàng #{0}", order.Id);
                    var body = bodyTemplate;
                    var address = string.Format("{0}{1}{2}", order.Address, (order.District != null ? ", " + order.District.Name : ""), (order.Province != null ? ", " + order.Province.Name : ""));

                    var orderdetail = new StringBuilder();
                    foreach (var detail in order.ProductOrders)
                    {
                        var row = rowTemplate.Replace("{productname}", detail.Product.Name);
                        row = row.Replace("{price}", detail.Price.ToString("N0"));
                        row = row.Replace("{quatity}", detail.Quatity.ToString("N0"));
                        row = row.Replace("{sum}", (detail.Quatity * detail.Price).ToString("N0"));

                        orderdetail.Append(row);
                    }

                    body = body.Replace("{username}", order.CustomerName);
                    body = body.Replace("{address}", address);
                    body = body.Replace("{email}", order.Email);
                    body = body.Replace("{phone}", order.Phone);
                    body = body.Replace("{note}", order.Note);
                    body = body.Replace("{orderid}", order.Id.ToString());
                    body = body.Replace("{orderdate}", order.OrderDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    body = body.Replace("{orderdetail}", orderdetail.ToString());
                    body = body.Replace("{total}", order.ProductOrders.Sum(a => a.Price * a.Quatity).ToString("N0"));

                    if (settings.IsSendEmailToAdmin)
                        EmailSender.InstantSend(subject, body, settings.DefaultSender, settings.AdminEmail);

                    if (settings.IsSendEmailToAdmin)
                        EmailSender.InstantSend(subject, body, settings.DefaultSender, order.Email);
                }
                catch (Exception exp)
                {
                    exp.Log("Problem send order mail");
                }
            }
        }

        protected void GetOrderInfo(Order order, string orderSessionId)
        {
            var path = Globals.MapPath("~/Userfiles/Templates/Order-Info.cshtml");
            var rowPath = Globals.MapPath("~/Userfiles/Templates/Order-Info-Row.cshtml");

            if (order != null && System.IO.File.Exists(path))
            {
                var bodyTemplate = System.IO.File.ReadAllText(path);
                var rowTemplate = "";

                if (System.IO.File.Exists(rowPath))
                    rowTemplate = System.IO.File.ReadAllText(rowPath);

                var settings = SiteConfiguration.GetConfig();

                try
                {

                    var body = bodyTemplate;
                    var address = string.Format("{0}{1}{2}", order.Address, (order.District != null ? ", " + order.District.Name : ""), (order.Province != null ? ", " + order.Province.Name : ""));

                    var orderdetail = new StringBuilder();
                    foreach (var detail in order.ProductOrders)
                    {
                        var row = rowTemplate.Replace("{productname}", detail.Product.Name);
                        row = row.Replace("{price}", detail.Price.ToString("N0"));
                        row = row.Replace("{quatity}", detail.Quatity.ToString("N0"));
                        row = row.Replace("{sum}", (detail.Quatity * detail.Price).ToString("N0"));

                        orderdetail.Append(row);
                    }

                    body = body.Replace("{username}", order.CustomerName);
                    body = body.Replace("{address}", address);
                    body = body.Replace("{email}", order.Email);
                    body = body.Replace("{phone}", order.Phone);
                    body = body.Replace("{note}", order.Note);
                    body = body.Replace("{orderid}", order.Id.ToString());
                    body = body.Replace("{orderdate}", order.OrderDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    body = body.Replace("{orderdetail}", orderdetail.ToString());
                    body = body.Replace("{total}", order.ProductOrders.Sum(a => a.Price * a.Quatity).ToString("N0"));

                    Session[orderSessionId] = body;
                }
                catch (Exception exp)
                {
                    exp.Log("Problem get order info");
                }
            }
        }
    }
}
