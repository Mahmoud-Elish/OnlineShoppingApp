using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Shopping.BL;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IConfiguration _configuration;

    public RedisCacheService(IConnectionMultiplexer redis, IConfiguration configuration)
    {
        _redis = redis;
        _configuration = configuration;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            
            if (value.HasValue)
                return JsonSerializer.Deserialize<T>(value);
                
            return default;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var db = _redis.GetDatabase();
            var serializedValue = JsonSerializer.Serialize(value);

            if (expiration == null)
            {
                var hours = _configuration.GetValue<int>("Redis:ExpirationTimeInHours");
                expiration = TimeSpan.FromHours(hours);
            }

            await db.StringSetAsync(key, serializedValue, expiration);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}

//Config Attach in Program.cs
public class RedisSettings
{
    public string ConnectionString { get; set; }
    public bool AbortOnConnectFail { get; set; }
}
public static class RedisServiceExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = new RedisSettings();
        configuration.GetSection("Redis").Bind(redisSettings);

        var options = ConfigurationOptions.Parse(redisSettings.ConnectionString);
        options.AbortOnConnectFail = redisSettings.AbortOnConnectFail;

        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(options));
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}