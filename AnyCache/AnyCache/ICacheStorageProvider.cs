using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace AnyCache
{
    /// <summary>
    /// Cache storage provider
    /// </summary>
    public interface ICacheStorageProvider : IDisposable
    {
        /// <summary>
        /// Set cache item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        void Set(string key, object item, MemoryCacheEntryOptions policy);

        /// <summary>
        /// Get item from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Get item from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Get item from cache, or add it if it doesn't exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        T GetOrCreate<T>(string key, Func<ICacheEntry, T> func);

        /// <summary>
        /// Get item from cache, or add it if it doesn't exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> func);

        /// <summary>
        /// Remove an item from cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}