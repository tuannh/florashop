using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FloraShop.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Sort ascending list by propertyName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        {
            return OrderBy<T>(source, propertyName, SortDirection.Ascending);
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName, SortDirection direction)
        {
            SortCondition condition = new SortCondition() { PropertyName = propertyName, SortDirection = direction };
            return OrderBy<T>(source, condition);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCondition oderbyCondition)
        {
            if (oderbyCondition != null)
                return OrderBy<T>(source, oderbyCondition, null);

            return source;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCondition oderbyCondition, params SortCondition[] thenByConditions)
        {
            if (oderbyCondition != null)
            {
                IOrderedEnumerable<T> srcTmp = null;

                if (oderbyCondition.SortDirection == SortDirection.Ascending)
                    srcTmp = source.OrderBy(x => GetPropertyValue(x, oderbyCondition.PropertyName));
                else
                    srcTmp = source.OrderByDescending(x => GetPropertyValue(x, oderbyCondition.PropertyName));

                if (thenByConditions != null)
                {
                    foreach (SortCondition condition in thenByConditions)
                    {
                        if (condition.SortDirection == SortDirection.Ascending)
                            srcTmp = srcTmp.ThenBy(a => GetPropertyValue(a, condition.PropertyName));
                        else
                            srcTmp = srcTmp.ThenByDescending(a => GetPropertyValue(a, condition.PropertyName));
                    }
                }

                return srcTmp;
            }

            return source;
        }

        private static object GetPropertyValue(object obj, string property)
        {
            string[] parts = property.Split('.');
            object value = null;

            if (parts.Length == 1)
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
                if (propertyInfo != null)
                    value = propertyInfo.GetValue(obj, null);
            }
            else
            {
                object tmpObj = obj;
                PropertyInfo propertyInfo = null;

                foreach (string part in parts)
                {
                    if (tmpObj != null)
                    {
                        propertyInfo = tmpObj.GetType().GetProperty(part);
                        if (propertyInfo != null)
                        {
                            value = propertyInfo.GetValue(tmpObj, null);
                            tmpObj = value;
                        }
                    }
                }
            }

            return value;
        }

        public static IQueryable<TProperty> DistinctProperty<TEntity, TProperty>(this IQueryable<TEntity> input, string propertyName)
             where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            var body = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<TEntity, TProperty>>(body, parameter);
            var result = input.Select(lambda).Distinct();
            return result;
        }
       
        /*-------------- call "DistinctBy" like bellow code -------check custom like :var resultSystem= listDistinct.Distinct();-------------------*/
        //List<Role> listDistinct = DataService.GetAll<Role>();        
        //var resulDistinct = Distinct.DistinctBy<Role, string>(listDistinct, x => x.RoleName);
        /*-----------------------------------------------------------------------------------------------*/
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public class SortCondition
    {
        public string PropertyName { get; set; }
        public SortDirection SortDirection { get; set; }
    }

}
