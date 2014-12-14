using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FloraShop.Core.BaseObjects
{
    public class SiteControl
    {
        private static SiteControl instance;
        public static SiteControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new SiteControl();

                return instance;
            }
        }

        internal HtmlHelper HtmlHelper { get; private set; }

        internal void SetHtmlHelper(HtmlHelper htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
        }

    }
}
