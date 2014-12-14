using EFCache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.DAL
{
    public class Configuration : DbConfiguration
    {
        public Configuration()
        {
            var transactionHandler = new CacheTransactionHandler(new EFCache.InMemoryCache());

            AddInterceptor(transactionHandler);

            Loaded +=
              (sender, args) => args.ReplaceService<DbProviderServices>(
                (s, _) => new CachingProviderServices(s, transactionHandler));
        }
    }
}
