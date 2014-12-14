
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace FloraShop.Core
{
    /// <summary>
    /// Summary description for CMSCache.
    /// </summary>
    public static class SiteCache
    {
        private static readonly Cache _cache = HttpRuntime.Cache;

        #region public static void Insert(...) overloads

        /// <summary>
        /// Insert the current "obj" into the cache for cache time from stie setttings or a weeking
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Insert(string key, object obj)
        {
            int durations = 0;

            //SiteContext ctx = SiteContext.Current;
            //if (ctx != null && ctx.IsWebRequest)
            //    durations = ctx.CurrentSite.Settings.CacheTimeout;

            durations = durations > 0 ? durations : 604800;  // 60 * 60 * 24 * 7 seconds = a week

            Insert(key, obj, durations);
        }

        public static void Insert(string key, object obj, CacheDependency dep)
        {
            key = GetClawKey(key);

            if (obj != null)
                _cache.Insert(key, obj, dep);
        }

        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, seconds, CacheItemPriority.Normal);
        }

        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, dep, seconds, priority, null);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            if (obj != null)
            {
                key = GetClawKey(key);
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds((double)seconds), Cache.NoSlidingExpiration, priority, onRemoveCallback);
            }
        }

        #endregion

        public static object Get(string key)
        {
            key = GetClawKey(key);
            return _cache.Get(key);
        }

        public static T Get<T>(string key) where T : class
        {
            object obj = Get(key);

            if (obj is T)
                return (T)obj;

            return null;
        }

        /// <summary>
        /// Removes the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            key = GetClawKey(key);
            _cache.Remove(key);
        }

        /// <summary>
        /// Removes all cache item by the by pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                {
                    _cache.Remove(enumerator.Key.ToString());
                }
            }
        }

        /// <summary>
        /// Clears all cache item
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Key);
            }
            foreach (string str in list)
            {
                _cache.Remove(str);
            }
        }

        /// <summary>
        /// Determines whether [contains cache entry] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool ContainsCacheEntry(string key)
        {
            key = GetClawKey(key);

            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            Regex regex = new Regex(key, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while (enumerator.MoveNext())
            {
                if (string.Compare(key, enumerator.Key.ToString(), true) == 0)
                {
                    return true;
                }
            }

            return false;

        }

        private static string GetClawKey(string key)
        {
            if (!key.StartsWith("Claw-Cache:"))
                key = string.Format("Claw-Cache:{0}", key);

            return key;
        }
    }
}