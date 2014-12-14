using System.Configuration;

namespace FloraShop.Core.Web.Mvc.Bundle.Configuration
{
    public class BundleDirectoryConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("directoryPath", IsRequired = true, IsKey = true)]
        public string DirectoryPath
        {
            get { return (string)this["directoryPath"]; }
            set { this["directoryPath"] = value; }
        }

        [ConfigurationProperty("searchPattern", IsRequired = true)]
        public string SearchPattern
        {
            get { return (string)this["searchPattern"]; }
            set { this["searchPattern"] = value; }
        }

        [ConfigurationProperty("searchSubdirectories", DefaultValue = false, IsRequired = false)]
        public bool SearchSubdirectories
        {
            get { return (bool)this["searchSubdirectories"]; }
            set { this["searchSubdirectories"] = value; }
        }

        [ConfigurationProperty("throwIfNotExist", DefaultValue = false, IsRequired = false)]
        public bool ThrowIfNotExist
        {
            get { return (bool)this["throwIfNotExist"]; }
            set { this["throwIfNotExist"] = value; }
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