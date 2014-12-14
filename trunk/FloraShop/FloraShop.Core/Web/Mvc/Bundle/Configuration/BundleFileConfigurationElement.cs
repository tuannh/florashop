using System.Configuration;

namespace FloraShop.Core.Web.Mvc.Bundle.Configuration
{
    public class BundleFileConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("filePath", IsRequired = true, IsKey = true)]
        public string FilePath
        {
            get { return (string)this["filePath"]; }
            set { this["filePath"] = value; }
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