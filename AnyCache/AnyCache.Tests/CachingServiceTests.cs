using AnyCache.Tests.TestData;
using NUnit.Framework;

namespace AnyCache.Tests
{
    [TestFixture]
    public class CachingServiceTests
    {
        private CachingService _cacheService;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _cacheService = new CachingService();
        }

        [Test]
        public void CachingService_MemoryCacheProvider_ShouldCache()
        {
            var key = nameof(CachingService_MemoryCacheProvider_ShouldCache);
            
            Assert.IsNull(_cacheService.Get<TestObject>(key));
            
            _cacheService.Add(key, new TestObject { Name = "Test", Value = 1024 });
            
            var val = _cacheService.Get<TestObject>(key);
            Assert.NotNull(val);
            Assert.AreEqual("Test", val.Name);
            Assert.AreEqual(1024, val.Value);
            // cacheService.Remove(key);
        }
    }
}
