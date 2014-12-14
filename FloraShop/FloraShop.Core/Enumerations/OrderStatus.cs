using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Enumerations
{
    public enum OrderStatus : int
    {
        None = 0,
        New = 1,
        Confirm = 2,
        Delivery = 3,
        Cancel = 4
    }
}
