using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Web.Mvc.WebResources
{
    public class FileElement
    {
        /// <summary>
        /// Virtual file path
        /// </summary>
        public string VirtualPath { get; set; }

        public FileElement()
        { }

        public FileElement(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string GetRelativePath()
        {
            if (!string.IsNullOrEmpty(this.VirtualPath))
            {
                System.Web.VirtualPathUtility.ToAbsolute(this.VirtualPath);
            }
            return "";
        }
    }
}
