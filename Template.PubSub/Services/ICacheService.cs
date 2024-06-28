using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Template.PubSub.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null);
        Task DeleteAsync(string key);
        Task DeleteAsync(List<string> keys);
        Task<List<string>> ClearCachePattern(string pattern);
        Task<List<string>> GetListKey(string pattern);
    }

    public class DistributedCacheService : ICacheService
    {
        private readonly ILogger<DistributedCacheService> _logger;
        private readonly IDistributedCache _database;

        public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
        {
            _logger = logger;
            _database = cache;
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                await _database.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + " " + ex.StackTrace);
            }
        }

        public async Task DeleteAsync(List<string> keys)
        {
            try
            {
                foreach (var x in keys)
                {
                    await DeleteAsync(x);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + " " + ex.StackTrace);
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var result = await _database.GetStringAsync(key);
                if (!string.IsNullOrEmpty(result))
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAsync = " + key);
                _logger.LogError(ex, ex.Message + " " + ex.StackTrace);

                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            try
            {
                expirationTime = expirationTime == null ? new TimeSpan(24, 0, 0) : expirationTime;
                await _database.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions() { SlidingExpiration = expirationTime });
            }
            catch (Exception ex)
            {
                _logger.LogError("SetAsync key= " + key);
                _logger.LogError(ex, ex.Message + " " + ex.StackTrace);
            }
        }

        public async Task<List<string>> GetListKey(string pattern)
        {
            try
            {
                string url = "localhost:6379,abortConnect=false";
                var flushedKeys = new List<string>();
                var options = StackExchange.Redis.ConfigurationOptions.Parse(url);
                options.AllowAdmin = true;
                var connection = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(options);
                using (connection)
                {
                    foreach (var endPoint in connection.GetEndPoints())
                    {
                        var server = connection.GetServer(endPoint);
                        var keys = server.Keys(0, pattern);
                        foreach (var key in keys)
                        {
                            flushedKeys.Add(key.ToString());
                        }
                    }
                }

                return flushedKeys;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when Get List Key {ex.Message}");
            }
            return new List<string>();
        }

        public async Task<List<string>> ClearCachePattern(string pattern)
        {
            var listKeys = await GetListKey(pattern);
            _ = DeleteAsync(listKeys);
            return listKeys;
        }
    }
}
