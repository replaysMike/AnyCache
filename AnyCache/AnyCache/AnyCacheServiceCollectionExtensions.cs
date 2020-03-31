using AnyCache.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AnyCache
{
    public static class AnyCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddLazyCache(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
            services.TryAdd(ServiceDescriptor.Singleton<ICacheStorageProvider, MemoryCacheProvider>());

            services.TryAdd(ServiceDescriptor.Singleton<IAppCache, CachingService>(serviceProvider =>
                new CachingService(
                    new Lazy<ICacheStorageProvider>(serviceProvider.GetRequiredService<ICacheStorageProvider>))));

            return services;
        }

        public static IServiceCollection AddLazyCache(this IServiceCollection services,
            Func<IServiceProvider, CachingService> implementationFactory)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
            services.TryAdd(ServiceDescriptor.Singleton<ICacheStorageProvider, MemoryCacheProvider>());

            services.TryAdd(ServiceDescriptor.Singleton<IAppCache>(implementationFactory));

            return services;
        }
    }
}
