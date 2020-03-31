using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace AnyCache.Providers
{
    /// <summary>
    /// In-memory caching provider
    /// </summary>
    public class MemoryCacheProvider : ICacheStorageProvider
    {
        internal readonly IMemoryCache _cache;

        public MemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set(string key, object item, MemoryCacheEntryOptions policy)
        {
            _cache.Set(key, item, policy);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory)
        {
            return _cache.GetOrCreate<T>(key, factory);
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> factory)
        {
            return _cache.GetOrCreateAsync<T>(key, factory);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _cache.Dispose();
            }
        }
    }
}