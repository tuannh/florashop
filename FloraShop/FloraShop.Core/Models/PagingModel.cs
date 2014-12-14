using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Models
{
    public class PagingModel
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get
            {
                if (ItemsPerPage <= 0 || TotalItems <= 0)
                {
                    return 0;
                }
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }

        public string PageIndexQueryKeyName { get; set; }
        public string RequestUrl { get; set; }

        public int PageCount { get; set; }

        public bool ShowFirstLast { get; set; }
        public bool ShowNextPrevious { get; set; }
        public bool ShowSummaryInfo { get; set; }
        public bool ShowAllPages { get; set; }
        public bool ShowCompactLink { get; set; }

        public string SummaryInfoTemplate { get; set; }

        public string CurrentPageCssClass { get; set; }
        public string PageNumberCssClass { get; set; }
        public string FirstPageNumberCssClass { get; set; }
        public string LastPageNumberCssClass { get; set; }

        public string FirstPageLinkCssClass { get; set; }
        public string LastPageLinkCssClass { get; set; }
        public string NextPageCssClass { get; set; }
        public string PrePageCssClass { get; set; }
        public string CompactPageLinkCssClass { get; set; }

        public string FirstPageText { get; set; }
        public string LastPageText { get; set; }
        public string NextPageText { get; set; }
        public string PrevPageText { get; set; }

        public PagingModel()
        {
            CurrentPageCssClass = "selected";
            PageNumberCssClass = "page-item";
            FirstPageNumberCssClass = "first";
            LastPageNumberCssClass = "last";
            FirstPageLinkCssClass = "go-first";
            LastPageLinkCssClass = "go-last";
            NextPageCssClass = "go-next";
            PrePageCssClass = "go-prev";
            CompactPageLinkCssClass = "go-compact";

            PageIndexQueryKeyName = "p";

            ItemsPerPage = 5;
            PageCount = 5;

            ShowAllPages = false;
            ShowSummaryInfo = true;
            ShowFirstLast = true;
            ShowNextPrevious = true;
            ShowCompactLink = true;

            FirstPageText = "First";
            LastPageText = "Last";
            NextPageText = "Next";
            PrevPageText = "Prev";
        }

        public PagingModel(string classSetting)
            : this()
        {
            if (!string.IsNullOrEmpty(classSetting))
            {
                var cssClasses = classSetting.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (cssClasses != null && cssClasses.Length > 0)
                {
                    foreach (var item in cssClasses)
                    {
                        var classPair = item.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        if (classPair.Length == 2)
                            SetValue(classPair[0], classPair[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Sets custom css value to paging control 
        /// </summary>
        /// <param name="key">The key get from PagingCssKey const class.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            if (string.Compare(key, PagingCssKey.CurrentPageCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.CurrentPageCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.PageNumberCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.PageNumberCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.FirstPageNumberCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.FirstPageNumberCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.LastPageNumberCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.LastPageNumberCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.FirstPageLinkCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.FirstPageLinkCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.LastPageLinkCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.LastPageLinkCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.NextPageCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.NextPageCssClass = value.Trim();
                    return;
                }
            }
            if (string.Compare(key, PagingCssKey.PrePageCssClass, true) == 0)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.PrePageCssClass = value.Trim();
                    return;
                }
            }
        }
    }

    public class PagingCssKey
    {
        public const string CurrentPageCssClass = "CurrentPage";
        public const string PageNumberCssClass = "PageNumber";
        public const string FirstPageNumberCssClass = "FirstPageNumber";
        public const string LastPageNumberCssClass = "LastPageNumber";
        public const string FirstPageLinkCssClass = "FirstPageLink";
        public const string LastPageLinkCssClass = "LastPageLink";
        public const string NextPageCssClass = "NextPage";
        public const string PrePageCssClass = "PrePage";
    }
}
