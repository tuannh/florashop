using FloraShop.Core.DAL;
using FloraShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FloraShop.Core.Controllers
{
    public class AdminController : BaseController
    {
        protected int PageIndex
        {
            get
            {
                var key = SiteContext.QueryString["p"] ?? "0";
                var pIndex = 0;
                int.TryParse(key, out pIndex);

                return pIndex > 0 ? pIndex : 1;
            }
        }

        protected int PageSize
        {
            get
            {
                var key = SiteContext.QueryString["ps"] ?? "0";
                var pSize = 0;
                int.TryParse(key, out pSize);

                return pSize > 0 ? pSize : 10;
            }
        }

        public AdminController(FloraShopContext  dbContext)
            : base(dbContext)
        {

        }
    }
}
