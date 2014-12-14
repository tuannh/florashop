using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class ProductPhoto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FileName { get; set; }

        public int DisplayOrder { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product  { get; set; }
    }
}
