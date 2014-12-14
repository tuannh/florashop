using FloraShop.Core.Extensions;
using FloraShop.Core.Interfaces;
using FloraShop.Core.Configurations;
using Ninject;
using Ninject.Modules;
using FloraShop.Core.Logs;
using System.Linq;
using System;
using FloraShop.Core.BaseObjects;
using FloraShop.Core.DAL;
using Ninject.Web.Common;

namespace FloraShop.Core.Configurations
{
    public class DbNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogProvider>().To<LogProvider>();
            Bind<SiteUrls>().To<SiteUrls>().InSingletonScope();
            Bind<SiteControl>().To<SiteControl>().InSingletonScope();

            Bind<FloraShopContext>().To<FloraShopContext>().InRequestScope();
        }
    }
}