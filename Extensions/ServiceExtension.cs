using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LaudCache.Extensions;

public static class ServiceExtension
{
    public static void AddRedis(this IServiceCollection services, string host, int port = 6379,
        int defaultDatabase = 0, bool ssl = false, string instanceName = "")
    {
        var redisConfigurations = new ConfigurationOptions
        {
            DefaultDatabase = defaultDatabase,
            EndPoints =
            {
                $"{host}:{port}",
            },
            Ssl = ssl
        };

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = redisConfigurations;
            options.InstanceName = instanceName;
        });
    }
}