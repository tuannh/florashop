using FloraShop.Core.DAL;
using FloraShop.Core;
using FloraShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using FloraShop.Core.Controllers;
using System.Web.Mvc;

namespace FloraShop.Web.Controllers
{
    public class FloraController : FrontController
    {
        public FloraController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

        [HttpPost]
        public ActionResult Add(string productId)
        {
            var session = SiteContext.Current.Context.Session;
            var lst = session[MyCart.ShopCart] as List<MyCart>;
            var msg = "";

            if (lst == null)
            {
                lst = new List<MyCart>();
                session[MyCart.ShopCart] = lst;
            }

            var cart = lst.FirstOrDefault(a => a.ProductId == productId);
            if (cart == null)
            {
                var db = DbContext;
                var id = int.Parse(productId);

                var product = db.Products.Find(id);
                if (product != null)
                {
                    cart = new MyCart(product);
                    lst.Add(cart);
                }
            }

            cart.Quatity++;
            var total = lst.Sum(a => a.Quatity * a.Price);
            var count = lst.Sum(a => a.Quatity);

            return Json(new { error = 0, message = msg, count = count.ToString("N0"), total = total.ToString("N0") }); ;
        }

        [HttpPost]
        public ActionResult Remove(string productId)
        {
            var session = SiteContext.Current.Context.Session;
            var lst = session[MyCart.ShopCart] as List<MyCart>;
            var rowId = string.Format("#tr{0}", productId);
            var total = 0.0;
            var count = 0;

            if (lst != null)
            {
                var cart = lst.FirstOrDefault(a => a.ProductId == productId);
                if (cart != null)
                {
                    lst.Remove(cart);

                    total = lst.Sum(a => a.Quatity * a.Price);
                    count = lst.Sum(a => a.Quatity);

                    return Json(new { error = 0, message = "", rowid = rowId, count = count.ToString("N0"), total = total.ToString("N0") });
                }
            }

            return Json(new { error = 1, message = "Sản phẩm không tồn tại trong giỏ hàng", rowid = rowId, count = count.ToString("N0"), total = total.ToString("N0") });
        }

        [HttpPost]
        public ActionResult Update(int id, int quatity)
        {
            var session = SiteContext.Current.Context.Session;
            var lst = session[MyCart.ShopCart] as List<MyCart>;

            var sum = 0.0;
            var total = 0.0;
            var count = 0;

            if (lst != null)
            {
                var item = lst.FirstOrDefault(a => a.ProductId == id.ToString());
                if (item != null)
                {
                    item.Quatity = quatity;
                    sum = item.Price * quatity;
                    total = lst.Sum(a => a.Price * a.Quatity);
                    count = lst.Sum(a => a.Quatity);

                    return Json(new { error = 0, message = "", rowid = string.Format("#tr{0}", id), total = total.ToString("N0"), sum = sum.ToString("N0"), count = count.ToString("N0") });
                }
            }

            return Json(new { error = 1, message = "Sản phẩn không tồn tại trong giỏ hàng", rowid = string.Format("#tr{0}", id), total = total.ToString("N0"), sum = sum.ToString("N0"), count = count.ToString("N0") });
        }

        [HttpPost]
        public ActionResult Clear()
        {
            var session = SiteContext.Current.Context.Session;
            session[MyCart.ShopCart] = null;

            return Json(new { error = 0, message = "Giỏ hàng rỗng", rowid = "", total = "0", sum = "0", count = "0" });
        }

        [HttpPost]
        public ActionResult District(int provinceId)
        {
            var db = new FloraShopContext();
            var lst = db.Districts.Where(a => a.ProvinceId == provinceId).ToList();

            var data = @"<option value=""0"">Chọn quận/huyện</option>";
            if (lst != null && lst.Count() > 0)
            {
                lst.ForEach(a => data += string.Format(@"<option value=""{0}"">{1}</option>", a.Id, a.Name));

                db.Dispose();

                return Json(new { error = 0, message = "", data = data });
            }

            return Json(new { error = 1, message = "Không tìm thấy quận/huyện", data = data });
        }

        [HttpPost]
        public ActionResult ValidUser(string username)
        {
            var db = SiteContext.Current.DbContext;
            var user = db.Users.Where(a => string.Compare(a.Username, username, true) == 0).FirstOrDefault();

            return Json(new { valid = (user != null ? 0 : 1) }); ;
        }

        [HttpPost]
        public ActionResult ValidEmail(string email)
        {
            var db = SiteContext.Current.DbContext;
            var user = db.Users.Where(a => string.Compare(a.Email, email, true) == 0).FirstOrDefault();

            return Json(new { valid = (user != null ? 0 : 1) }); ;
        }
    }
}
