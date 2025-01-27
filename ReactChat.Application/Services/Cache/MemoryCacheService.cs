using Microsoft.Extensions.Caching.Memory;
using ReactChat.Application.Interfaces.Cache;

namespace ReactChat.Application.Services.Cache
{
    public class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
        private readonly IMemoryCache _cache = cache;
        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out T? value))
                return await Task.FromResult<T?>(value);
            return await Task.FromResult<T?>(default);
        }
        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan expiration)
        {
            _cache.Set(cacheKey, value, expiration);
            await Task.CompletedTask;
        }
        public async Task RemoveAsync(string cacheKey)
        {
            _cache.Remove(cacheKey);
            await Task.CompletedTask;
        }
    }
}
