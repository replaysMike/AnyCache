# AnyCache
AnyCache is a modified clone of [LazyCache](https://github.com/alastairtree/LazyCache) for caching any .Net type, with multiple storage providers available. It provides an in-memory cache that leverages Microsoft.Extensions.Caching and provides performance and reliability in heavy load scenarios.

## Installation

```Powershell
PM> Install-Package AnyCache
```

## Getting Started

```csharp
// This follows the exact same interface as LazyCache. Create or inject (DI) your caching service
IAppCache cache = new CachingService();

// Get our existing item in the cache by referencing its key, and provide the factory to create a new object if it's not in the cache
var myObjectInstance = cache.GetOrAdd<MyObject>("uniqueKey", () => methodThatTakesTimeOrResources());
```

## Features

* Super simple usage with sliding or absolute expiration
* Thread safe, concurrency ready
* Async compatible (where LazyCache had some issues)
* Supports dependency injection
* Strongly typed generics and object based support

## Providers

* [Redis distributed storage](https://github.com/replaysMike/AnyCache.Providers.Redis)
