using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnyCache.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace AnyCache
{
    /// <summary>
    /// Caching service
    /// </summary>
    public class CachingService : IAppCache
    {
        private readonly Lazy<ICacheStorageProvider> _cacheProvider;

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Get the current cache provider
        /// </summary>
        public virtual ICacheStorageProvider CacheProvider => _cacheProvider.Value;

        /// <summary>
        /// Policy defining how long items should be cached for unless specified
        /// </summary>
        public virtual CacheDefaults DefaultCachePolicy { get; set; } = new CacheDefaults();

        public static Lazy<ICacheStorageProvider> DefaultCacheProvider { get; set; }
            = new Lazy<ICacheStorageProvider>(() =>
                new MemoryCacheProvider(
                    new MemoryCache(
                        new MemoryCacheOptions())
                ));

        /// <summary>
        /// Create a caching service using the default MemoryCacheProvider
        /// </summary>
        public CachingService() : this(DefaultCacheProvider)
        {
        }

        /// <summary>
        /// Create a caching service using a specified <seealso cref="ICacheStorageProvider"/>
        /// </summary>
        /// <param name="cacheProvider"></param>
        public CachingService(Lazy<ICacheStorageProvider> cacheProvider)
        {
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        /// <summary>
        /// Create a caching service using a specified <seealso cref="ICacheStorageProvider"/> factory
        /// </summary>
        /// <param name="cacheProviderFactory"></param>
        public CachingService(Func<ICacheStorageProvider> cacheProviderFactory)
        {
            if (cacheProviderFactory == null) throw new ArgumentNullException(nameof(cacheProviderFactory));
            _cacheProvider = new Lazy<ICacheStorageProvider>(cacheProviderFactory);
        }

        /// <summary>
        /// Create a caching service using a specified <seealso cref="ICacheStorageProvider"/>
        /// </summary>
        /// <param name="cache"></param>
        public CachingService(ICacheStorageProvider cache) : this(() => cache)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        public virtual void Add<T>(string key, T item, MemoryCacheEntryOptions policy)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);

            CacheProvider.Set(key, item, policy);
        }

        public virtual void Add<T>(string key, T item, DateTimeOffset expires)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);

            CacheProvider.Set(key, item, new MemoryCacheEntryOptions { AbsoluteExpiration = expires });
        }

        public virtual void Add<T>(string key, T item, TimeSpan slidingExpiration)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);

            CacheProvider.Set(key, item, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        }

        public virtual object Get(string key)
        {
            ValidateKey(key);

            return CacheProvider.Get(key);
        }

        public virtual T Get<T>(string key)
        {
            ValidateKey(key);

            return CacheProvider.Get<T>(key);
        }

        public virtual T GetOrAdd<T>(string key, Func<ICacheEntry, T> addItemFactory)
        {
            ValidateKey(key);

            T cacheItem;

            _lock.Wait();
            try
            {
                cacheItem = CacheProvider.GetOrCreate<T>(key, (entry) =>
                {
                    // thread safety is up to the provider
                    var result = addItemFactory(entry);
                    EnsureEvictionCallbackDoesNotReturnTheAsyncOrLazy<T>(entry.PostEvictionCallbacks);
                    return result;
                });
            }
            finally
            {
                _lock.Release();
            }

            return cacheItem;
        }

        public virtual void Remove(string key)
        {
            ValidateKey(key);
            CacheProvider.Remove(key);
        }

        public virtual async Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> addItemFactory)
        {
            ValidateKey(key);

            T cacheItem;

            await _lock.WaitAsync().ConfigureAwait(false);
            try
            {
                cacheItem = await CacheProvider.GetOrCreateAsync<T>(key, async (entry) =>
                {
                    // thread safety is up to the provider
                    var result = await addItemFactory(entry);
                    EnsureEvictionCallbackDoesNotReturnTheAsyncOrLazy<T>(entry.PostEvictionCallbacks);
                    return result;
                });
            }
            finally
            {
                _lock.Release();
            }

            return cacheItem;
        }

        protected virtual void EnsureEvictionCallbackDoesNotReturnTheAsyncOrLazy<T>(IList<PostEvictionCallbackRegistration> callbackRegistrations)
        {
            if (callbackRegistrations != null)
                foreach (var item in callbackRegistrations)
                {
                    var originalCallback = item.EvictionCallback;
                    item.EvictionCallback = (key, value, reason, state) =>
                    {
                        // before the original callback we need to unwrap the Lazy that holds the cache item
                        if (value is Lazy<T> cacheItem)
                            value = cacheItem.IsValueCreated ? cacheItem.Value : default(T);

                        // pass the unwrapped cached value to the original callback
                        originalCallback(key, value, reason, state);
                    };
                }
        }

        protected virtual void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentOutOfRangeException(nameof(key), "Cache keys cannot be empty or whitespace");
        }
    }
}