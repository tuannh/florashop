using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class Page
    {
        public int Id { get; set; }

        public string UniqueKey { get; set; }

        public string Title {get; set;}

        public string Name { get; set; }

        public string Alias { get; set; }

        public bool IsDefault { get; set; }

        public bool Active { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }

        public string Layout { get; set; }
    }
}
