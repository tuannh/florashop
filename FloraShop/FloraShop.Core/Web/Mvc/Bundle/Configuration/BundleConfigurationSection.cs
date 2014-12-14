using System.Configuration;
using System.IO;
using System.Xml;

namespace FloraShop.Core.Web.Mvc.Bundle.Configuration
{
    public class BundleConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("ignoreIfDebug", DefaultValue = true, IsRequired = false)]
        public bool IgnoreIfDebug
        {
            get { return (bool)this["ignoreIfDebug"]; }
            set { this["ignoreIfDebug"] = value; }
        }

        [ConfigurationProperty("ignoreIfLocal", DefaultValue = true, IsRequired = false)]
        public bool IgnoreIfLocal
        {
            get { return (bool)this["ignoreIfLocal"]; }
            set { this["ignoreIfLocal"] = value; }
        }

        [ConfigurationProperty("jsBundles", IsDefaultCollection = false)]
        public BundleConfigurationElementCollection JsBundles
        {
            get { return base["jsBundles"] as BundleConfigurationElementCollection; }
        }

        [ConfigurationProperty("cssBundles", IsDefaultCollection = false)]
        public BundleConfigurationElementCollection CssBundles
        {
            get { return base["cssBundles"] as BundleConfigurationElementCollection; }
        }

        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            base.DeserializeSection(reader);
        }

        protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
        {
            return base.SerializeSection(parentElement, name, saveMode);
        }

        #region IStandaloneConfigurationSection Members

        public void DeserializeSection(string config)
        {
            this.DeserializeSection(new XmlTextReader(new StringReader(config)));
        }

        #endregion
    }
}