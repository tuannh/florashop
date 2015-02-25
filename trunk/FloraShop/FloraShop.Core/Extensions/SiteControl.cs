using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FloraShop.Core.Extensions;
using FloraShop.Core.Models;
using FloraShop.Core.Utility;
using System.Web;
using FloraShop.Core.BaseObjects;
using FloraShop.Core.DAL;
using FloraShop.Core.Domain;
using FloraShop.Core.Enumerations;
using System.Web.UI.WebControls;
using FloraShop.Core.Search;

namespace FloraShop.Core.Extensions
{
    public static class SiteControlExtensions
    {
        public static MvcHtmlString Pager(this SiteControl control, PagingModel pagingInfo)
        {
            if (pagingInfo.TotalPages <= 1)
                return new MvcHtmlString("");

            #region recheck page info

            var defaultInfo = new PagingModel();
            if (pagingInfo.ItemsPerPage <= 0)
                pagingInfo.ItemsPerPage = defaultInfo.ItemsPerPage;

            if (pagingInfo.PageCount <= 0)
                pagingInfo.PageCount = defaultInfo.PageCount;

            if (pagingInfo.CurrentPage <= 0 || pagingInfo.CurrentPage > pagingInfo.TotalPages)
                pagingInfo.CurrentPage = 1;

            var startPage = 0;
            if (pagingInfo.CurrentPage % pagingInfo.PageCount == 0)
                startPage = (int)(pagingInfo.CurrentPage / pagingInfo.PageCount) * pagingInfo.PageCount - pagingInfo.PageCount + 1;
            else
                startPage = (int)(pagingInfo.CurrentPage / pagingInfo.PageCount) * pagingInfo.PageCount + 1;

            var endPage = startPage + pagingInfo.PageCount - 1;
            if (endPage > pagingInfo.TotalPages)
                endPage = pagingInfo.TotalPages;

            #endregion

            var result = new StringBuilder();

            if (pagingInfo.ShowAllPages)
            {
                #region Enable full paging

                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    var tag = new TagBuilder("a");
                    var cssClass = "";

                    if (i == 1)
                        cssClass = !string.IsNullOrEmpty(pagingInfo.FirstPageNumberCssClass) ? pagingInfo.FirstPageNumberCssClass : defaultInfo.FirstPageNumberCssClass;

                    else if (i == pagingInfo.CurrentPage)
                        cssClass = !string.IsNullOrEmpty(pagingInfo.CurrentPageCssClass) ? pagingInfo.CurrentPageCssClass : defaultInfo.CurrentPageCssClass;

                    else
                        cssClass = !string.IsNullOrEmpty(pagingInfo.PageNumberCssClass) ? pagingInfo.PageNumberCssClass : defaultInfo.PageNumberCssClass;

                    tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, i.ToString()));
                    tag.InnerHtml = i.ToString();
                    tag.AddCssClass(cssClass);

                    result.Append(tag.ToString());
                }

                #endregion
            }
            else
            {
                #region show paging with current paging length settings

                for (int i = startPage; i <= endPage; i++)
                {
                    var tag = new TagBuilder("a");
                    var cssClass = "";
                    var href = "";

                    if (i == pagingInfo.CurrentPage)
                    {
                        cssClass = !string.IsNullOrEmpty(pagingInfo.CurrentPageCssClass) ? pagingInfo.CurrentPageCssClass : defaultInfo.CurrentPageCssClass;
                        href = "javascript:void(0);";
                    }
                    else
                    {
                        cssClass = !string.IsNullOrEmpty(pagingInfo.PageNumberCssClass) ? pagingInfo.PageNumberCssClass : defaultInfo.PageNumberCssClass;
                        href = Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, i.ToString());
                    }

                    tag.MergeAttribute("href", href);
                    tag.InnerHtml = i.ToString();
                    tag.AddCssClass(cssClass);

                    result.Append(tag.ToString());
                }

                #endregion
            }

            var controlBuilder = new StringBuilder();

            #region show first/preview/compact link

            #region  Show first Page

            if (pagingInfo.CurrentPage > 1 && pagingInfo.ShowFirstLast)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.FirstPageLinkCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.FirstPageLinkCssClass) && string.Compare(defaultInfo.FirstPageLinkCssClass, pagingInfo.FirstPageLinkCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.FirstPageLinkCssClass);
                }

                tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, "1"));
                tag.InnerHtml = pagingInfo.FirstPageText;

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region Show previous Page

            if (pagingInfo.CurrentPage > 1 && pagingInfo.ShowNextPrevious)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.PrePageCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.PrePageCssClass) && string.Compare(defaultInfo.PrePageCssClass, pagingInfo.PrePageCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.PrePageCssClass);
                }

                tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, (pagingInfo.CurrentPage - 1).ToString()));
                tag.InnerHtml = pagingInfo.PrevPageText;

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region show compact link

            if (pagingInfo.ShowCompactLink && startPage > 1)
            {
                var tag = new TagBuilder("a");
                var css = !string.IsNullOrEmpty(pagingInfo.CompactPageLinkCssClass) ? pagingInfo.CompactPageLinkCssClass : defaultInfo.FirstPageLinkCssClass;
                var pageIndex = (startPage - 1).ToString();

                tag.AddCssClass(css);
                tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, pageIndex));
                tag.InnerHtml = "paging_CompactLink".Localize("...");

                controlBuilder.Append(tag.ToString());
            }

            if (controlBuilder.Length > 0)
                result.Insert(0, controlBuilder);

            #endregion

            #endregion

            #region show compact/next/last link

            controlBuilder = new StringBuilder();

            #region show compact link

            if (pagingInfo.ShowCompactLink && endPage < pagingInfo.TotalPages)
            {
                var tag = new TagBuilder("a");
                var css = !string.IsNullOrEmpty(pagingInfo.CompactPageLinkCssClass) ? pagingInfo.CompactPageLinkCssClass : defaultInfo.FirstPageLinkCssClass;
                var pageIndex = (endPage + 1).ToString();

                tag.AddCssClass(css);
                tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, pageIndex));
                tag.InnerHtml = "paging_CompactLink".Localize("...");

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region Show next Page

            if (pagingInfo.CurrentPage < pagingInfo.TotalPages && pagingInfo.ShowNextPrevious)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.NextPageCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.NextPageCssClass) && string.Compare(defaultInfo.NextPageCssClass, pagingInfo.NextPageCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.NextPageCssClass);
                }

                tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, (pagingInfo.CurrentPage + 1).ToString()));
                tag.InnerHtml = pagingInfo.NextPageText;

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region Show last Page

            if (pagingInfo.CurrentPage < pagingInfo.TotalPages && pagingInfo.ShowFirstLast)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.LastPageLinkCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.LastPageLinkCssClass) && string.Compare(defaultInfo.LastPageLinkCssClass, pagingInfo.LastPageLinkCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.LastPageLinkCssClass);
                }

                tag.MergeAttribute("href", Globals.AppendQueryStringValue(pagingInfo.RequestUrl, pagingInfo.PageIndexQueryKeyName, pagingInfo.TotalPages.ToString()));
                tag.InnerHtml = pagingInfo.LastPageText;

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            if (controlBuilder.Length > 0)
                result.Append(controlBuilder);

            #endregion

            #region show summary Info

            if (pagingInfo.ShowSummaryInfo)
            {
                var infoTemplate = pagingInfo.SummaryInfoTemplate;
                infoTemplate = !string.IsNullOrEmpty(infoTemplate) ? infoTemplate : "paging_summaryInfoText".Localize("Page {0} of {1} pages ({2} items)");

                var summary = string.Format(infoTemplate, pagingInfo.CurrentPage, pagingInfo.TotalPages, pagingInfo.TotalItems);
                result.Insert(0, summary);
            }

            #endregion

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString Slider(this SiteControl control, BannerType type, string viewName = "Slider.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();
            var category = (int)type;

            var banners = db.Banners.Where(a => a.Active && a.Category == category).OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name).ToList();

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, banners);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString MenuNavigator(this SiteControl control, string viewName = "MenuNavigator.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var pages = db.Pages.Where(p => p.Active).ToList();
            var viewBag = control.HtmlHelper.ViewContext.ViewBag;

            viewBag.ProductCategory = db.Categories.Where(a => a.Active && !a.ParentId.HasValue).ToList();

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, pages);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString CategoryList(this SiteControl control, string viewName = "CategoryList.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();
            var routeData = SiteContext.Current.RouteData;

            var cateAlias = routeData.Values["categoryalias"] as string;

            var currentCategory = db.Categories.Where(a => string.Compare(a.Alias, cateAlias, true) == 0).FirstOrDefault();


            var rootCategories = db.Categories
                                   .Where(p => p.Parent == null && p.Active)
                                   .OrderBy(p => p.DisplayOrder)
                                   .ThenBy(p => p.Name)
                                   .ToList();

            control.HtmlHelper.ViewContext.ViewBag.Category = currentCategory;

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, rootCategories);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString ProductList(this SiteControl control, string viewName = "ProductList.cshtml")
        {
            #region get params

            var ctx = SiteContext.Current;
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);

            var db = new FloraShopContext();
            var ps = ctx.QueryString["ps"] ?? "8";
            var pi = ctx.QueryString["p"] ?? "1";
            var sortField = ctx.QueryString["sort"] ?? "Price";
            var sortDirect = ctx.QueryString["order"] ?? "desc";
            var kw = ctx.QueryString["kw"] ?? "";
            var strType = ctx.QueryString["type"] ?? "0";

            var dirct = string.Compare(sortDirect, "desc", true) == 0 ? SortDirection.Descending : SortDirection.Ascending;
            var pagesize = 4;
            var pageindex = 1;
            var type = 0;

            int.TryParse(ps, out pagesize);
            int.TryParse(pi, out pageindex);
            int.TryParse(strType, out type);

            var routeData = SiteContext.Current.RouteData;
            var cateAlias = routeData.Values["categoryalias"] != null ? routeData.Values["categoryalias"].ToString() : "";
            var brandAlias = routeData.Values["brandAlias"] != null ? routeData.Values["brandAlias"].ToString() : "";

            #endregion

            var lst = SearchService.Search(kw, cateAlias, brandAlias, type);

            var total = 0;
            if (lst != null)
            {
                total = lst.Count();
                lst = lst.OrderBy(sortField, dirct).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }

            var pagingModel = new PagingModel();
            pagingModel.ItemsPerPage = pagesize;
            pagingModel.CurrentPage = pageindex;
            pagingModel.TotalItems = total;
            pagingModel.RequestUrl = ctx.RawUrl;

            control.HtmlHelper.ViewContext.ViewBag.PagingModel = pagingModel;
            control.HtmlHelper.ViewContext.ViewBag.Category = db.Categories.Where(a => string.Compare(a.Alias, cateAlias, true) == 0).FirstOrDefault();
            control.HtmlHelper.ViewContext.ViewBag.Brand = db.Brands.Where(a => string.Compare(a.Alias, brandAlias, true) == 0).FirstOrDefault();


            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, lst);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString LastViewProduct(this SiteControl control, string viewName = "LastViewProduct.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var lst = new List<Product>();
            var db = new FloraShopContext();

            var cookie = Globals.GetCookie("LastIds") ?? "";

            var arrIds = cookie.Replace("&", "").Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrIds.Length > 4)
            {
                var tmpIds = arrIds.Take(4).Select(a => string.Format("&{0}|", a)).ToArray();
                cookie = string.Join("", tmpIds);
                Globals.SetCookie("LastIds", cookie);
            }
            if (arrIds.Length > 0)
            {
                var lstIds = arrIds.Select(a => int.Parse(a)).ToList();
                lst = db.Products.Where(p => lstIds.Contains(p.Id)).ToList();
            }

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, lst);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString ProductDetail(this SiteControl control, string viewName = "ProductDetail.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var routeData = SiteContext.Current.RouteData;
            var alias = routeData.Values["productalias"] != null ? routeData.Values["productalias"].ToString() : "";
            var cateAlias = routeData.Values["categoryAlias"] != null ? routeData.Values["categoryAlias"].ToString() : "";

            var product = db.Products
                            .FirstOrDefault(p => p.Active && string.Compare(p.Alias, alias, true) == 0);

            var category = db.Categories.Where(a => string.Compare(a.Alias, cateAlias, true) == 0).FirstOrDefault();

            if (product != null)
            {
                var relateProducts = db.Products.Where(a => a.CategoryId == product.CategoryId).OrderBy(a => a.DislayOrder).Take(8).ToList();
                control.HtmlHelper.ViewContext.ViewBag.RelateProducts = relateProducts;
            }

            control.HtmlHelper.ViewContext.ViewBag.Category = category;

            var result = "";
            if (product != null)
            {
                var cookie = Globals.GetCookie("LastIds") ?? "";
                var curId = string.Format("&{0}|", product.Id);
                cookie = cookie.Replace(curId, ""); // remove old value
                cookie = curId + cookie;

                Globals.SetCookie("LastIds", cookie);
                
                result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, product);

                db.Dispose();
            }
            else
            {
                SiteContext.Current.Context.Response.Redirect("/san-pham/", true);
            }
           
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString CategoryBreadcum(this SiteControl control, string viewName = "CategoryBreadcum.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var cateAlias = "";
            var routeData = SiteContext.Current.RouteData;
            if (routeData != null && routeData.Values != null && routeData.Values["categoryalias"] != null)
                cateAlias = routeData.Values["categoryalias"].ToString();

            var cate = db.Categories.FirstOrDefault(p => p.Active && string.Compare(p.Alias, cateAlias, true) == 0);

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, cate);

            return new MvcHtmlString(result);
        }

        public static MvcHtmlString BrandList(this SiteControl control, int numberShow = int.MaxValue, string viewName = "BrandList.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var lst = db.Brands
                        .Where(p => p.Active)
                        .OrderBy(p => p.DisplayOrder)
                        .ThenBy(p => p.Name)
                        .Take(numberShow)
                        .ToList();

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, lst);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString UserGuide(this SiteControl control, string viewName = "UserGuide.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var lst = db.UserGuides.Where(p => p.Active)
                             .OrderBy(p => p.DisplayOrder)
                             .ThenBy(p => p.Name)
                             .ToList();

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, lst);

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString UserGuideDetail(this SiteControl control, string viewName = "UserGuideDetail.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var userguide = "";
            var routeData = SiteContext.Current.RouteData;
            if (routeData != null && routeData.Values != null && routeData.Values["userguide"] != null)
                userguide = routeData.Values["userguide"].ToString();

            var result = "";
            var obj = db.UserGuides.FirstOrDefault(a => string.Compare(a.Alias, userguide, true) == 0 && a.Active);

            if (obj != null)
            {
                result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, obj);
            }

            db.Dispose();
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString Content(this SiteControl control, string contentName, string viewName = "Content.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = new FloraShopContext();

            var obj = db.Contents.FirstOrDefault(a => string.Compare(a.Alias, contentName, true) == 0 || string.Compare(a.PageAlias, contentName, true) == 0);

            var result = "";
            if (obj != null)
            {
                result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, obj);
            }

            db.Dispose();

            return new MvcHtmlString(result);
        }

        public static MvcHtmlString PromotionList(this SiteControl control, string viewName = "PromotionList.cshtml")
        {
            var viewPath = string.Format("~/Views/Shared/Controls/{0}", viewName);
            var db = SiteContext.Current.DbContext;
            var ctx = SiteContext.Current;

            var ps = ctx.QueryString["ps"] ?? "8";
            var pi = ctx.QueryString["p"] ?? "1";

            var pagesize = 4;
            var pageindex = 1;
            var total = 0;

            int.TryParse(ps, out pagesize);
            int.TryParse(pi, out pageindex);

            var products = db.Products.Where(a => a.Active && a.SalePrice > 0).ToList();
            if (products != null)
            {
                total = products.Count();
                products = products.Skip((pageindex - 1) * pagesize)
                                   .Take(pagesize)
                                   .OrderBy(a => a.DislayOrder).ToList();
            }

            var pagingModel = new PagingModel();
            pagingModel.ItemsPerPage = pagesize;
            pagingModel.CurrentPage = pageindex;
            pagingModel.TotalItems = total;
            pagingModel.RequestUrl = ctx.RawUrl;

            control.HtmlHelper.ViewContext.ViewBag.PagingModel = pagingModel;

            var result = RenderViewToString(control.HtmlHelper.ViewContext.Controller, viewPath, products);

            return new MvcHtmlString(result);
        }

        #region render view

        private static string RenderViewToString(ControllerBase controller, string viewPath, object model)
        {
            var path = Globals.MapPath(viewPath);

            if (File.Exists(path))
            {
                controller.ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    var view = new RazorView(controller.ControllerContext, viewPath, string.Empty, false, null);
                    var viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, sw);

                    view.Render(viewContext, sw);

                    return sw.ToString();
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
