
using FloraShop.Core.DAL;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraShop.Core.Domain
{
    public class Category
    {
        public Category()
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
        [Required(ErrorMessage = "Tên danh mục không thể rỗng")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [MaxLength, Column(TypeName = "ntext")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Hiển thị")]
        public bool Active { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }

        [Range(0, 9999, ErrorMessage = "Thứ tự hiển thị là 1 số trong khoảng [0, 9999]")]
        [RegularExpression(@"^[0-9]{0,4}$", ErrorMessage = "Thứ tự hiển thị là 1 số trong khoảng [0, 9999]")]
        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        public virtual Category Parent { get; set; }

        #region support method

        public IEnumerable<Category> GetSubCategory()
        {
            var db = new FloraShopContext();
            var lst = db.Categories
                        .Where(p => p.Parent != null && p.Parent.Id == this.Id && p.Active)
                        .OrderBy(p => p.DisplayOrder)
                        .ThenBy(p => p.Name)
                        .ToList();

            return lst;
        }

        public IEnumerable<Category> GetAllSubCategory()
        {
            var subList = GetSubCategory();
            var lst = new List<Category>();

            if (subList != null && subList.Count() > 0)
            {
                foreach (var cate in subList)
                {
                    lst.AddRange(cate.GetAllSubCategory());
                }

                lst.AddRange(subList);
            }

            return lst;
        }

        public bool HasProduct()
        {
            var db = new FloraShopContext();
            var product = db.Products
                        .Where(p => p.CategoryId == this.Id).FirstOrDefault();

            db.Dispose();

            return product != null;
        }

        public IEnumerable<Product> Products()
        {
            var db = new FloraShopContext();
            var product = db.Products
                        .Where(p => p.CategoryId == this.Id).ToList();

            db.Dispose();

            return product;
        }

        public bool IsValidAlias()
        {
            var db = new FloraShopContext();
            var cate = db.Categories.SingleOrDefault(a => string.Compare(a.Alias, this.Alias, true) == 0);
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
            var cate = db.Categories.SingleOrDefault(a => string.Compare(a.Name, this.Name, true) == 0);
            db.Dispose();

            // add new
            if (this.Id == 0)
                return cate == null;

            // update 
            return cate == null || cate.Id == this.Id;
        }

        public bool IsActive(string cateAlias)
        {
            var lst = GetAllSubCategory();
            if (lst != null && lst.Count() > 0)
            {
                var cate = lst.FirstOrDefault(a => string.Compare(a.Alias, cateAlias, true) == 0);

                return cate != null;
            }

            return false;
        }

        #endregion
    }
}
