using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime? GetDate(this DateTime date, string value, string format)
        {
            try
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch (Exception exp)
            {
                exp.Log();

                return null;
            }
        }
    }
}
