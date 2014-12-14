using FloraShop.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FloraShop.Web.Models
{
    public class MyCart
    {
        public const string ShopCart = "ShopCart";

        public MyCart(Product pro)
        {
            ProductId = pro.Id.ToString();
            Name = pro.Name;
            Code = pro.Code;
            Photo = pro.GetPhoto() != null ? pro.GetPhoto().FileName : "";
            Alias = pro.Alias;
            Price = pro.SalePrice > 0 ? pro.SalePrice : pro.Price;
        }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string Photo { get; set; }

        public string Alias { get; set; }

        public float Price { get; set; }

        public int Quatity { get; set; }

        public double Sum { get { return Price * Quatity; } }
    }
}