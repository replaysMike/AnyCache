using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace AnyCache
{
    /// <summary>
    /// Application cache interface
    /// </summary>
    public interface IAppCache
    {
        /// <summary>
        /// Get the current cache provider
        /// </summary>
        ICacheStorageProvider CacheProvider { get; }

        /// <summary>
        /// The default cache policy, if no expiry is provided
        /// </summary>
        CacheDefaults DefaultCachePolicy { get; }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        void Add<T>(string key, T item, MemoryCacheEntryOptions policy);

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="expires"></param>
        void Add<T>(string key, T item, DateTimeOffset expires);

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="slidingExpiration"></param>
        void Add<T>(string key, T item, TimeSpan slidingExpiration);

        /// <summary>
        /// Get item from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Get or add item from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <returns></returns>
        T GetOrAdd<T>(string key, Func<ICacheEntry, T> addItemFactory);

        /// <summary>
        /// Get or add item from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="addItemFactory"></param>
        /// <returns></returns>
        Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> addItemFactory);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}