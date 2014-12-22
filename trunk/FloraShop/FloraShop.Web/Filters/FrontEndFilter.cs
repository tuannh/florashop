using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FloraShop.Web;
using FloraShop.Core;

namespace FloraShop.Web.Filters
{
    public sealed class FrontEndFilter : BaseFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var ctx = SiteContext.Current;
            if (!ctx.Context.Request.IsAuthenticated)
            {
                ctx.Context.Response.Redirect("/dang-nhap/");
            }
        }
    }
}
