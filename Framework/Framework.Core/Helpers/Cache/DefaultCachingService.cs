using Framework.Core.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;

namespace Framework.Core.Helpers.Cache
{
    public class DefaultCachingService : ICachingService
    {
        private readonly IMemoryCache _cache;
        private readonly IUserOrgInfoServices _userOrgInfoServices;
        private const int _timeToLive = 7;

        public DefaultCachingService(IMemoryCache cache, IUserOrgInfoServices userOrgInfoServices)
        {
            this._cache = cache;
            _userOrgInfoServices = userOrgInfoServices;
        }

        public void Clear(string key)
        {
            _cache.Remove(key);
        }

        public void ClearByKeyContainAsync(string key)
        {
            var orgId = _userOrgInfoServices._orgId != Guid.Empty ? _userOrgInfoServices._orgId.ToString() : Guid.Empty.ToString();
            orgId = _userOrgInfoServices._groupId != Guid.Empty ? _userOrgInfoServices._groupId.ToString() : orgId;
            var keyDelete = GetKeys().Where(x => x.ToLower().Contains(key) && x.ToLower().Contains(orgId)).ToArray();
            Task.WaitAll(ClearAsync(keyDelete));
        }

        public Task ClearAllAsync()
        {
            if (_cache is MemoryCache memoryCache)
            {
                var percentage = 1.0;//100%
                memoryCache.Compact(percentage);
            }
            return Task.CompletedTask;
        }

        public Task ClearAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task ClearAsync(string[] keys)
        {
            foreach (var item in keys)
            {
                _cache.Remove(item);
            }
            return Task.CompletedTask;
        }

        public T Get<T>(string key, Func<T> fallback)
        {
            var orgId = _userOrgInfoServices._orgId != Guid.Empty ? _userOrgInfoServices._orgId.ToString() : Guid.Empty.ToString();
            orgId = _userOrgInfoServices._groupId != Guid.Empty ? _userOrgInfoServices._groupId.ToString() : orgId;
            key = $"{orgId}_{key}";
            bool foundInCache = _cache.TryGetValue(key, out T value);
            if (foundInCache) return value;
            value = fallback();
            if(value != null)
                _cache.Set(key, value, TimeSpan.FromDays(_timeToLive));
            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> callback)
        {
            return await _cache.GetOrCreateAsync(key, async m =>
            {
                return await Task.FromResult(await callback());
            });
        }

        public List<string> GetKeys()
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_cache) as ICollection;
            var items = new List<string>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    items.Add(val.ToString());
                }
            }
            return items;
        }
    }
}
