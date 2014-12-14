using FloraShop.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.DAL
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<FloraShopContext>
    {
        protected override void Seed(FloraShopContext context)
        {
            InitAdminUser(context);
        }

        private void InitAdminUser(FloraShopContext context)
        {
            var admin = new User()
            {
                Username = "admin",
                Password = "VhL+dONCCxu68w0qZiuRJuBJx5k=",
                PasswordSalt = "/+jj1XHAkCKH8MGbXSmETg==",
                Email = "nht257@yahoo.com",
                IsAdmin = true,
                CreatedDate = DateTime.Now,
                Active = true
            };
            context.Users.Add(admin);

            var webmaster = new User()
            {
                Username = "webmaster",
                Password = "UR+o5v1LPhotXzvrCakeUAvpAzQ=",
                PasswordSalt = "BbXwoC3mCjjwBFPWmDhNug==",
                Email = "redhearthcm@gmail.com",
                IsAdmin = true,
                CreatedDate = DateTime.Now,
                Active = true
            };
            context.Users.Add(webmaster);

            context.SaveChanges();
        }
    }
}
