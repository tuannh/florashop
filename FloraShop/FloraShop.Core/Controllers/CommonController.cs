﻿using FloraShop.Core.DAL;
using FloraShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FloraShop.Core.Controllers
{
    public class CommonController : BaseController
    {
        public CommonController(FloraShopContext dbContext)
            : base(dbContext)
        {

        }

    }
}
