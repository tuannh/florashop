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
    public class BaseController : Controller
    {
        public FloraShopContext DbContext { get; private set; }

        public SiteContext SiteContext
        {
            get;
            private set;
        }

        public BaseController(FloraShopContext dbContext)
        {
            DbContext = dbContext;
            SiteContext = SiteContext.Current;
        }

    }
}
