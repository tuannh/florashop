using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FloraShop.Core.Extensions;
using System.Runtime.Serialization.Formatters.Binary;

namespace FloraShop.Core.Utility
{
    public class CopyHelper
    {
        /// <summary>
        /// Deep copy an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static T DeepCopy<T>(T src)
        {
            var memoryStream = new System.IO.MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, src);


            memoryStream.Position = 0;
            T returnValue = (T)binaryFormatter.Deserialize(memoryStream);


            memoryStream.Close();
            memoryStream.Dispose();


            return returnValue;
        }

        /// <summary>
        /// Clone object with basic properties(value type, enum or string type). If property of object is object/complex types, it will be ignored.
        /// </summary>
        /// <param name="objSource">The source object</param>
        /// <returns></returns>
        public static object BasicCloneObject(object objSource)
        {
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);

            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.CanWrite)
                {
                    //
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                }
            }
            return objTarget;
        }

    }
}
