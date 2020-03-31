using System;
using Microsoft.Extensions.Caching.Memory;

namespace AnyCache
{
    /// <summary>
    /// Default cache options
    /// </summary>
    public class CacheDefaults
    {
        public virtual int DefaultCacheDurationSeconds { get; set; } = 60 * 30;

        internal MemoryCacheEntryOptions BuildOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(DefaultCacheDurationSeconds)
            };
        }
    }
}