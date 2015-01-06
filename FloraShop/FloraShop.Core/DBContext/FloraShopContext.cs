using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using FloraShop.Core.Domain;
using FloraShop.Core.Logs;

namespace FloraShop.Core.DAL
{
    public class FloraShopContext : DbContext
    {
        public FloraShopContext()
            : base("FloraShopDb")
        {
            Database.Log = EFLogger.EventLog;
        }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductPhoto> ProductPhotos { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGuide> UserGuides { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public void Initialize()
        {
            Database.SetInitializer<FloraShopContext>(new DatabaseInitializer());

            this.Database.Initialize(true);
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<UserPoint> UserPoints { get; set; }

    }
}
