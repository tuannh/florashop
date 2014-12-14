using System;
using System.Configuration;

namespace FloraShop.Core.Web.Mvc.Bundle.Configuration
{
    [ConfigurationCollection(typeof(BundleFileConfigurationElement))]
    public class BundleFileConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BundleFileConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((BundleFileConfigurationElement)element).FilePath;
        }

        public BundleFileConfigurationElement this[int index]
        {
            get { return (BundleFileConfigurationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }
    }
}