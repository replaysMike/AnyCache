using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace AnyCache
{
    public static class AppCacheExtensions
    {
        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public static void Add<T>(this IAppCache cache, string key, T item)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            cache.Add(key, item, cache.DefaultCachePolicy.BuildOptions());
        }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="expires"></param>
        public static void Add<T>(this IAppCache cache, string key, T item, DateTimeOffset expires)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            cache.Add(key, item, new MemoryCacheEntryOptions { AbsoluteExpiration = expires });
        }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="slidingExpiration"></param>
        public static void Add<T>(this IAppCache cache, string key, T item, TimeSpan slidingExpiration)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            cache.Add(key, item, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IAppCache cache, string key, Func<T> addItemFactory)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAdd(key, addItemFactory, cache.DefaultCachePolicy.BuildOptions());
        }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IAppCache cache, string key, Func<T> addItemFactory, DateTimeOffset expires)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAdd(key, addItemFactory, new MemoryCacheEntryOptions { AbsoluteExpiration = expires });
        }

        /// <summary>
        /// Get or add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IAppCache cache, string key, Func<T> addItemFactory, TimeSpan slidingExpiration)
        {
            return cache.GetOrAdd(key, addItemFactory, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        }

        /// <summary>
        /// Get or add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IAppCache cache, string key, Func<T> addItemFactory, MemoryCacheEntryOptions policy)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAdd(key, entry =>
            {
                entry.SetOptions(policy);
                return addItemFactory();
            });
        }

        /// <summary>
        /// Get or add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <returns></returns>
        public static Task<T> GetOrAddAsync<T>(this IAppCache cache, string key, Func<Task<T>> addItemFactory)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAddAsync(key, addItemFactory, cache.DefaultCachePolicy.BuildOptions());
        }

        /// <summary>
        /// Get or add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static Task<T> GetOrAddAsync<T>(this IAppCache cache, string key, Func<Task<T>> addItemFactory, DateTimeOffset expires)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAddAsync(key, addItemFactory, new MemoryCacheEntryOptions { AbsoluteExpiration = expires });
        }

        /// <summary>
        /// Get or add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public static Task<T> GetOrAddAsync<T>(this IAppCache cache, string key, Func<Task<T>> addItemFactory, TimeSpan slidingExpiration)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAddAsync(key, addItemFactory,
                new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        }

        /// <summary>
        /// Get or add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static Task<T> GetOrAddAsync<T>(this IAppCache cache, string key, Func<Task<T>> addItemFactory, MemoryCacheEntryOptions policy)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            return cache.GetOrAddAsync(key, entry =>
            {
                entry.SetOptions(policy);
                return addItemFactory();
            });
        }
    }
}