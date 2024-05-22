//using System.Collections;
//using System.Reflection;
//using System.Text;
//using System.Text.Json;

//namespace Framework.Core.Helpers.Cache
//{
//    public class RedisCacheService : ICachingService
//    {
//        private readonly IDistributedCache _cache;
//        private const int _timeToLive = 7;

//        public RedisCacheService(IDistributedCache cache)
//        {
//            this._cache = cache;
//        }

//        public void Clear(string key)
//        {
//            _cache.Remove(key);
//        }

//        public Task ClearAllAsync()
//        {
//            if (_cache is IDistributedCache memoryCache)
//            {
//                var percentage = 1.0;//100%
//                memoryCache.(percentage);
//            }
//            return Task.CompletedTask;
//        }

//        public Task ClearAsync(string key)
//        {
//            _cache.Remove(key);
//            return Task.CompletedTask;
//        }

//        public Task ClearAsync(string[] keys)
//        {
//            foreach (var item in keys)
//            {
//                _cache.Remove(item);
//            }
//            return Task.CompletedTask;
//        }

//        public async Task SetAsync<T>(string key, T data)
//        {
//            var options = new DistributedCacheEntryOptions();

//            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(60);

//            var jsonData = JsonSerializer.Serialize(data);
//            await _cache.SetStringAsync(key, jsonData, options);
//        }

//        public async Task<T> GetAsync<T>(string key, Func<Task<T>> callback)
//        {
//            var jsonData = await _cache.GetStringAsync(key);

//            if (jsonData is null)
//            {
//                return default(T);
//            }

//            return JsonSerializer.Deserialize<T>(jsonData);
//        }

//        public List<string> GetKeys()
//        {
//            string[] keysArr = _cache.ke;

//            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
//            var collection = field.GetValue(_cache) as ICollection;
//            var items = new List<string>();
//            if (collection != null)
//            {
//                foreach (var item in collection)
//                {
//                    var methodInfo = item.GetType().GetProperty("Key");
//                    var val = methodInfo.GetValue(item);
//                    items.Add(val.ToString());
//                }
//            }
//            return items;
//        }

//        public T Get<T>(string key, Func<T> fallback)
//        {
//            string? serializedData = null;
//            var dataAsByteArray = _cache.Get(key);
//            if ((dataAsByteArray?.Count() ?? 0) <= 0)
//                return default(T);

//            serializedData = Encoding.UTF8.GetString(dataAsByteArray);
//            return JsonSerializer.Deserialize<T>(serializedData);
//        }

//        public void ClearByKeyContainAsync(string key)
//        {
//            var keyDelete = GetKeys().Where(x => x.ToLower().Contains(key)).ToArray();
//            Task.WaitAll(ClearAsync(keyDelete));
//        }

//    }
//}
