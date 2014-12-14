using FloraShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Search
{
    public class SearchItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public float Price { get; set; }

        public float SalePrice { get; set; }
    }
}
