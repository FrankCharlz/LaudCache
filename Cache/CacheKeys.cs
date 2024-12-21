using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;

namespace LaudCache.Cache;

public class CacheKeys
{
    // max size 10,000
    public static readonly ConcurrentDictionary<string, DistributedCacheEntryOptions> KeysConcurrentDictionary = new();
}