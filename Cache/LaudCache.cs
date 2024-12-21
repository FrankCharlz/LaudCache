using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LaudCache.Cache;

public class LaudCache: ILaudCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<LaudCache> _logger;
    private readonly JsonSerializerSettings _serializerSettings;

    public LaudCache(IDistributedCache cache, ILogger<LaudCache> logger)
    {
        _cache = cache;
        _logger = logger;
        _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }


    public T? Get<T>(string key)
    {
        string? value = _cache.GetString(key);
        return !string.IsNullOrEmpty(value) ? JsonConvert.DeserializeObject<T>(value) : default;
    }

    public void Add<T>(string key, T value, TimeSpan duration, bool isSliding = true)
    {
        string serializedValue = JsonConvert.SerializeObject(value, _serializerSettings);
        _cache.SetString(key, serializedValue, GetCacheOptions(duration, isSliding));
    }

    private static DistributedCacheEntryOptions GetCacheOptions(TimeSpan expiration, bool isSliding)
    {
        return isSliding 
            ? new DistributedCacheEntryOptions { SlidingExpiration = expiration } 
            : new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration };
    }

    public T GetOrAdd<T>(string key, Func<Task<T>> valueFactory, TimeSpan duration, bool isSliding = true)
    {

        
        T? value = Get<T>(key);
        if (value != null) return value;
        
        value =  valueFactory.Invoke().Result;
        Add(key, value, duration, isSliding);
        return value;
        
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan duration)
    {
        
        T? value = Get<T>(key);
        if (value != null) return value;
        
        value = await valueFactory.Invoke();
        if (value != null) Add(key, value, duration);
        
        return value;
    }
    
    public async Task RemoveKeyAsync(string key)
    {
        await _cache.RemoveAsync(key);
        _logger.LogDebug("Invalidated cache with key {0}", key);

    }
}