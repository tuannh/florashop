using FloraShop.Core;
using FloraShop.Core.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Logs
{
    public class EFLogger
    {
        public static void EventLog(string message)
        {
            if (SiteConfiguration.GetConfig().LogEvent == false)
                return;

            var path = string.Format("EF.{0}.log", DateTime.Now.ToString("yyyy.MM.dd"));
            path = Path.Combine("~/logs/", path);
            path = Globals.MapPath(path);

            var content = string.Format(@"Event date/time: {0}{1}Event message: {2}{3}------------------------------------------------------------------------------------------------------------------------{3}{3}",
                                            DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss fff"), Environment.NewLine,
                                            message, Environment.NewLine);

            try
            {
                File.AppendAllText(path, content);
            }
            catch { }
        }
    }
}
