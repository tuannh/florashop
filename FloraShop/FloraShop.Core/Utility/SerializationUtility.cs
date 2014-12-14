﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FloraShop.Core.Extensions;

namespace FloraShop.Core.Utility
{
    public class SerializationUtility
    {
        #region Serialize

        /// <summary>
        /// Serializes the specified o.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="filePath">The file path.</param>
        public static void Serialize<T>(T o, string filePath)
        {
            string folderPath = Path.GetDirectoryName(filePath);
            IOUtility.EnsureDirectoryExists(folderPath);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Serialize(o, stream);
            }
        }

        /// <summary>
        /// Serializes the specified o.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="stream">The stream.</param>
        public static void Serialize<T>(T o, Stream stream)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings()
               {
                   Indent = true,
                   IndentChars = "\t"
               };
            XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
            using (var writer = XmlWriter.Create(xmlWriter, settings))
            {
                ser.WriteObject(writer, o);
            }
        }

        /// <summary>
        /// Serializes as XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        public static string SerializeAsXml<T>(T o, IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null)
                knownTypes = new[] { typeof(T) };

            try
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter xmlWriter = XmlWriter.Create(sb))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
                    ser.WriteObject(xmlWriter, o);
                }
                return sb.ToString();
            }
            catch (Exception exp)
            {
                exp.Log();
            }

            return string.Empty;
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes the specified file path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            return (T)Deserialize(typeof(T), new[] { typeof(T) }, filePath);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static object Deserialize(Type type, IEnumerable<Type> knownTypes, string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Deserialize(type, knownTypes, stream);
            }
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static object Deserialize(Type type, IEnumerable<Type> knownTypes, Stream stream)
        {
            DataContractSerializer ser = new DataContractSerializer(type, knownTypes);
            return ser.ReadObject(stream);
        }

        /// <summary>
        /// Deserializes from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string xml, IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null)
            {
                knownTypes = new[] { typeof(T) };
            }
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
                    return (T)ser.ReadObject(reader);
                }
            }
            catch (Exception exp)
            {
                exp.Log();
            }

            return default(T);
        }

        public static T DeserializeFromXmlNode<T>(XmlNode xmlNode, IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null)
            {
                knownTypes = new[] { typeof(T) };
            }
            try
            {
                T objTarget = Activator.CreateInstance<T>();
                var xml = SerializeAsXml<T>(objTarget);
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                doc.DocumentElement.InnerXml = xmlNode.InnerXml;
                if (doc != null && !string.IsNullOrEmpty(doc.InnerXml))
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(doc.OuterXml)))
                    {
                        DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
                        return (T)ser.ReadObject(reader);
                    }
                }
            }
            catch (Exception exp)
            {
                exp.Log();
            }

            return default(T);
        }
        #endregion
    }
}
