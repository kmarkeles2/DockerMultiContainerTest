using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache _cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public string? Get()
        {
            string? forecast = null;
            string cacheKey = "CurrentForecast";
            try
            {
                forecast = _cache.GetString(cacheKey);
                if (forecast == null)
                {
                    forecast = GetRandomWeather();
                    _cache.SetString(cacheKey, forecast, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(5)});
                }
            }
            catch (RedisConnectionException)
            {
                forecast = "Redis cache is not found.";
            }
            return forecast;
        }
        private static string GetRandomWeather()
        {
            var random = new Random();

            int randomIndex = random.Next(Summaries.Length);
            return Summaries[randomIndex];
        }
    }
}
