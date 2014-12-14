using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Enumerations
{
    public enum PageType : int
    {
        None = 0,
        Home,
        Product,
        ProductDetail,
        BuyGuide,
        Promotion,
        Contact,
        Shopcart,
        Order,
        Login,
        Register
    }
}
