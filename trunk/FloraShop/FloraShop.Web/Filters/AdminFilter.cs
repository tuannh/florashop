using FloraShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace FloraShop.Web.Filters
{
    public sealed class AdminFilter : BaseFilter
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var ctx = SiteContext.Current;
            if (!ctx.User.IsAdmin)
            {
                FormsAuthentication.SignOut();
                ctx.Context.Response.Redirect("/common/user/login/");
            }
        }
    }
}
