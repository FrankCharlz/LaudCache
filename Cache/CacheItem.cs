using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace LaudCache.Cache;

public class CacheItem
{
    public List<KeyValuePair<string, StringValues>> headers { get; set; }
    public string body { get; set; }

    public string GetJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static CacheItem FromJson(string json)
    {
        return JsonConvert.DeserializeObject<CacheItem>(json);
    }
}