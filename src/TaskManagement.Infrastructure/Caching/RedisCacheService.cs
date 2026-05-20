using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(
        IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedData =
            await _cache.GetStringAsync(key);

        if (cachedData is null)
            return default;

        return JsonSerializer.Deserialize<T>(
            cachedData);
    }

    public async Task SetAsync<T>(string cacheKey, T value)
    {
        var options =
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

        var jsonData =
            JsonSerializer.Serialize(value);
       
        await _cache.SetStringAsync(
            cacheKey,
            jsonData,
            options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}