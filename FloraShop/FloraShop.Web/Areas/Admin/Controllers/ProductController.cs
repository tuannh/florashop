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
using FloraShop.Core;
using System.IO;
using FloraShop.Core.Utility;
using System.Drawing;
using FloraShop.Core.Models;
using FloraShop.Core.Controllers;
using FloraShop.Core.Search;
using FloraShop.Core.Extensions;
using FloraShop.Web.Filters;

namespace FloraShop.Web.Areas.Admin.Controllers
{
    [AdminFilter]
    public class ProductController : AdminController
    {

        public const string Folder = "~/Userfiles/Modules/Products/";
        public const string ThumbFolder = "~/Userfiles/Modules/Products/Thumb/";

        public const string TmpFolder = "~/Userfiles/_temp/Modules/Product/";
        public const string TmpThumbFolder = "~/Userfiles/_temp/Modules/Product/thumb/";

        public ProductController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

        // GET: Admin/Product
        public ActionResult Index(string kw, int? type, int? catid, int? brandid)
        {
            var lst = DbContext.Products.ToList();

            if (type.HasValue && type.Value > 0)
                lst = lst.Where(p => p.Type == type).ToList();

            if (catid.HasValue && catid.Value > 0)
            {
                lst = lst.Where(p => p.CategoryId == catid).ToList();
                ViewBag.Category = DbContext.Categories.Find(catid);
            }

            if (brandid.HasValue && brandid.Value > 0)
            {
                lst = lst.Where(p => p.BrandId == brandid).ToList();
                ViewBag.Brand = DbContext.Brands.Find(brandid);
            }

            if (!string.IsNullOrEmpty(kw))
            {
                var keyword = kw.ToLower().Trim();

                lst = lst.Where(a => a.Name.ToLower().Contains(keyword) ||
                                     (a.Code ?? "").ToLower().Contains(keyword) ||
                                     (a.MadeIn ?? "").ToLower().Contains(keyword) ||
                                     (a.Description ?? "").ToLower().Contains(keyword))
                         .OrderBy(a => a.DislayOrder)
                         .ThenByDescending(a => a.Id)
                         .ToList();

                if (lst.Count > 0)
                    ViewBag.SearchReseult = string.Format("<b>{0}</b> kết quả được tìm thấy", lst.Count);
                else
                    ViewBag.SearchReseult = string.Format("Không tìm thấy kết quả với từ khóa <b>{0}</b>", kw);
            }
            else
            {
                lst = lst.OrderBy(a => a.DislayOrder)
                         .ThenByDescending(a => a.Id)
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

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(DbContext.Brands, "Id", "Name");
            ViewBag.CategoryId = new SelectList(SelectCategoryList(), "Value", "Text");
            // ViewBag.CategoryId = new SelectList(DbContext.Categories, "Id", "Name");

            return View(new Product());
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Alias,Code,ShortDescription,Description,Active,Price,SalePrice,MadeIn,DislayOrder,CategoryId,BrandId,Type,Photos,Sizes,Colors")] Product product)
        {
            ViewBag.BrandId = new SelectList(DbContext.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(SelectCategoryList(), "Value", "Text", product.CategoryId);

            #region custom valid data

            if (!product.IsValidName())
            {
                ViewBag.Error = "Tên sản phẩm tồn tại";
                return View(product);
            }

            if (!product.IsValidAlias())
            {
                ViewBag.Error = "Alias tồn tại";
                return View(product);
            }

            if (!product.IsValidCode())
            {
                ViewBag.Error = "Mã sản phẩm tồn tại";
                return View(product);
            }

            #endregion

            if (ModelState.IsValid)
            {
                DbContext.Products.Add(product);
                var photos = product.Photos;
                DbContext.SaveChanges();

                if (photos != null)
                {
                    MakeFolder();

                    #region save photo

                    foreach (var photo in photos)
                    {
                        var tmpThumbPath = Globals.MapPath(TmpThumbFolder + photo.FileName);
                        var tmpPath = Globals.MapPath(TmpFolder + photo.FileName);
                        var ext = Path.GetExtension(photo.FileName);

                        photo.FileName = string.Format("{0}-Photo{1}{2}", product.Id, DateTime.Now.Ticks, ext);

                        var path = Globals.MapPath(Folder + photo.FileName);
                        var thumPath = Globals.MapPath(ThumbFolder + photo.FileName);

                        if (System.IO.File.Exists(tmpPath))
                            System.IO.File.Move(tmpPath, path);

                        if (System.IO.File.Exists(tmpThumbPath))
                            System.IO.File.Move(tmpThumbPath, thumPath);
                    }

                    #endregion

                    SearchService.AddUpdateLuceneIndex(product);

                    DbContext.SaveChanges();
                }

                return RedirectToAction("index");
            }

            return View(product);
        }

        private static void MakeFolder()
        {
            var folder = Globals.MapPath(Folder);
            var thumbFolder = Globals.MapPath(ThumbFolder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!Directory.Exists(thumbFolder))
                Directory.CreateDirectory(thumbFolder);
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                var folderPath = Globals.MapPath(TmpFolder);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var thumbFolderPath = Globals.MapPath(TmpThumbFolder);
                if (!Directory.Exists(thumbFolderPath))
                    Directory.CreateDirectory(thumbFolderPath);

                var title = Path.GetFileName(file.FileName);
                var filename = string.Format("tmp-{0}-{1}", DateTime.Now.Ticks, Path.GetFileName(file.FileName));
                var path = string.Format("{0}{1}", folderPath, filename);
                var thumbpath = string.Format("{0}{1}", thumbFolderPath, filename);

                var tmpname = string.Format("{0}-{1}", Guid.NewGuid().ToString(), Path.GetFileName(file.FileName));
                var tmppath = string.Format("{0}{1}", folderPath, tmpname);
                file.SaveAs(tmppath);

                var config = FloraShop.Core.Configurations.SiteConfiguration.GetConfig();
                var imgConfig = config.Product;
                ImageTools.FixResizeImage(tmppath, thumbpath, imgConfig.ThumbWidth, imgConfig.ThumbHeight, ColorTranslator.FromHtml(imgConfig.Background), config.Quality);
                ImageTools.FixResizeImage(tmppath, path, imgConfig.Width, imgConfig.Height, ColorTranslator.FromHtml(imgConfig.Background), config.Quality);

                try { System.IO.File.Delete(tmppath); }
                catch { }

                var obj = new { Error = 0, Message = "", title = title, displayorder = 1000, filename = filename };

                return Json(obj);
            }

            var errObj = new { Error = 1, FileName = "", Message = "" };

            return Json(errObj);
        }


        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DbContext.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.BrandId = new SelectList(DbContext.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(SelectCategoryList(), "Value", "Text", product.CategoryId);

            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Alias,Code,ShortDescription,Description,Active,Price,SalePrice,MadeIn,DislayOrder,CategoryId,BrandId,Type,Photos,Sizes,Colors")] Product product)
        {
            ViewBag.BrandId = new SelectList(DbContext.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(SelectCategoryList(), "Value", "Text", product.CategoryId);

            #region custom valid data

            if (!product.IsValidName())
            {
                ViewBag.Error = "Tên sản phẩm tồn tại";
                return View(product);
            }

            if (!product.IsValidAlias())
            {
                ViewBag.Error = "Alias tồn tại";
                return View(product);
            }

            if (!product.IsValidCode())
            {
                ViewBag.Error = "Mã sản phẩm tồn tại";
                return View(product);
            }

            #endregion

            if (ModelState.IsValid)
            {
                var photos = product.Photos;
                var delPhotos = new List<int>();

                if (photos != null)
                {
                    MakeFolder();

                    #region save photo

                    foreach (var photo in photos)
                    {
                        if (photo.Id > 0)
                            DbContext.Entry(photo).State = EntityState.Modified;
                        else if (photo.Id == 0)
                            DbContext.Entry(photo).State = EntityState.Added;
                        else if (photo.Id < 0)
                        {
                            photo.Id = Math.Abs(photo.Id);
                            delPhotos.Add(photo.Id);
                        }

                        if (!photo.FileName.StartsWith("tmp"))
                            continue;

                        photo.ProductId = product.Id;

                        var tmpThumbPath = Globals.MapPath(TmpThumbFolder + photo.FileName);
                        var tmpPath = Globals.MapPath(TmpFolder + photo.FileName);
                        var ext = Path.GetExtension(photo.FileName);

                        photo.FileName = string.Format("{0}-Photo{1}{2}", product.Id, DateTime.Now.Ticks, ext);

                        var path = Globals.MapPath(Folder + photo.FileName);
                        var thumPath = Globals.MapPath(ThumbFolder + photo.FileName);

                        if (System.IO.File.Exists(tmpPath))
                            System.IO.File.Move(tmpPath, path);

                        if (System.IO.File.Exists(tmpThumbPath))
                            System.IO.File.Move(tmpThumbPath, thumPath);
                    }
                }

                    #endregion

                DbContext.Entry(product).State = EntityState.Modified;
                DbContext.SaveChanges();

                // delete photo
                foreach (var id in delPhotos)
                {
                    var photo = DbContext.ProductPhotos.Find(id);

                    var path = Globals.MapPath(Folder + photo.FileName);
                    var thumPath = Globals.MapPath(ThumbFolder + photo.FileName);

                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);

                        if (System.IO.File.Exists(thumPath))
                            System.IO.File.Delete(thumPath);
                    }
                    catch (Exception exp)
                    {
                        exp.Log();
                    }

                    DbContext.ProductPhotos.Remove(photo);
                }

                SearchService.AddUpdateLuceneIndex(product);
                DbContext.SaveChanges();

                return RedirectToAction("index");
            }

            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = DbContext.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            if (product.Photos != null)
            {
                foreach (var photo in product.Photos)
                {
                    var path = Globals.MapPath(Folder + photo.FileName);
                    var thumPath = Globals.MapPath(ThumbFolder + photo.FileName);

                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);

                        if (System.IO.File.Exists(thumPath))
                            System.IO.File.Delete(thumPath);
                    }
                    catch (Exception exp)
                    {
                        exp.Log();
                    }
                }
            }


            SearchService.ClearRecordIndex(product);
            DbContext.Products.Remove(product);
            
            DbContext.SaveChanges();

            return RedirectToAction("index");
        }


        #region support methods

        private List<SelectListItem> SelectCategoryList()
        {
            var root = DbContext.Categories.Where(p => p.Parent == null)
                                    .OrderBy(p => p.DisplayOrder)
                                    .ThenBy(p => p.Name)
                                    .ToList();

            var selectList = new List<SelectListItem>();
            //var itemNone = new SelectListItem()
            //{
            //    Text = "None",
            //    Value = "0"
            //};
            //selectList.Add(itemNone);

            if (root != null)
            {
                foreach (var cate in root)
                {
                    var item = new SelectListItem()
                    {
                        Text = cate.Name,
                        Value = cate.Id.ToString()
                    };
                    selectList.Add(item);
                    SubCategoryList(selectList, cate, "----");
                }
            }

            return selectList;
        }

        private void SubCategoryList(List<SelectListItem> selectList, Category parent, string indexText)
        {
            var subCates = parent.GetSubCategory();

            if (subCates != null)
            {
                foreach (var cate in subCates)
                {
                    var item = new SelectListItem()
                    {
                        Text = string.Format("{0}{1}", indexText, cate.Name),
                        Value = cate.Id.ToString()
                    };
                    selectList.Add(item);
                    var newIndex = indexText + indexText;
                    SubCategoryList(selectList, cate, newIndex);
                }
            }

        }

        #endregion
    }
}
