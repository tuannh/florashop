
using FloraShop.Core.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FloraShop.Core.Domain
{
    public class Brand
    {
        public Brand()
        {
            DisplayOrder = 1000;
            Active = true;
        }

        public int Id { get; set; }

        [Display(Name = "Alias")]
        [Required(ErrorMessage = "Alias không thể rỗng")]
        [StringLength(100, ErrorMessage = "Alias không được quá 100 kí tự")]
        public string Alias { get; set; }

        [StringLength(100, ErrorMessage = "Tên không được quá 100 kí tự")]
        [Required(ErrorMessage = "Tên nhãn hiệu không thể rỗng")]
        [Display(Name = "Tên nhãn hiệu")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [MaxLength, Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Display(Name = "Hiển thị")]
        public bool Active { get; set; }

        [Range(0, 9999, ErrorMessage = "Thứ tự hiển thị là 1 số trong khoảng [0, 9999]")]
        [RegularExpression(@"^[0-9]{0,4}$", ErrorMessage = "Thứ tự hiển thị là 1 số trong khoảng [0, 9999]")]
        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }

        public bool IsValidAlias()
        {
            var db = new FloraShopContext();
            var cate = db.Brands.SingleOrDefault(a => string.Compare(a.Alias, this.Alias, true) == 0);
            db.Dispose();

            // add new
            if (Id == 0)
                return cate == null;

            // update 
            return cate == null || cate.Id == Id;
        }

        public bool IsValidName()
        {
            var db = new FloraShopContext();
            var cate = db.Brands.SingleOrDefault(a => string.Compare(a.Name, this.Name, true) == 0);
            db.Dispose();

            // add new
            if (this.Id == 0)
                return cate == null;

            // update 
            return cate == null || cate.Id == this.Id;
        }

        public bool HasProduct()
        {
            var db = new FloraShopContext();
            var product = db.Products
                        .Where(p => p.BrandId == this.Id).FirstOrDefault();

            db.Dispose();

            return product != null;
        }

        public IEnumerable<Product> Products()
        {
            var db = new FloraShopContext();
            var product = db.Products
                        .Where(p => p.BrandId == this.Id).ToList();

            db.Dispose();

            return product;
        }
    }
}