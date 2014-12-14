using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core
{
    public class IoC
    {
        public static StandardKernel Kernel
        {
            get;
            set;
        }
    }
}
