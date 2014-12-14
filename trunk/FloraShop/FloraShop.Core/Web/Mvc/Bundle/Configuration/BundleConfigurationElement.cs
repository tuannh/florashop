using System.Configuration;

namespace FloraShop.Core.Web.Mvc.Bundle.Configuration
{
    public class BundleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("bundlePath", IsRequired = true, IsKey = true)]
        public string BundlePath
        {
            get { return (string)this["bundlePath"]; }
            set { this["bundlePath"] = value; }
        }

        [ConfigurationProperty("cdnPath", IsRequired = false)]
        public string CdnPath
        {
            get { return (string)this["cdnPath"]; }
            set { this["cdnPath"] = value; }
        }

        [ConfigurationProperty("minify", DefaultValue = true, IsRequired = false)]
        public bool Minify
        {
            get { return (bool)this["minify"]; }
            set { this["minify"] = value; }
        }

        [ConfigurationProperty("files", IsDefaultCollection = false, IsRequired = false)]
        public BundleFileConfigurationElementCollection Files
        {
            get { return base["files"] as BundleFileConfigurationElementCollection; }
        }

        [ConfigurationProperty("directories", IsDefaultCollection = false, IsRequired = false)]
        public BundleDirectoryConfigurationElementCollection Directories
        {
            get { return base["directories"] as BundleDirectoryConfigurationElementCollection; }
        }

        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
        }


        protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey)
        {
            return base.SerializeElement(writer, serializeCollectionKey);
        }
    }
}