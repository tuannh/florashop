using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class UserPoint
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int OrderId { get; set; }

        public int Points { get; set; }

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }

        public virtual Order Order { get; set; }
    }
}
