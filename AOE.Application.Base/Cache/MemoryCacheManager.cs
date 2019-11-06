using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace AOE.Application.Base.Cache
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Set<T>(object key, T value) => _cache.Set(key, value);

        public T Set<T>(object key, T value, TimeSpan absoluteExpirationRelativeToNow) => _cache.Set(key, value, absoluteExpirationRelativeToNow);

        public T Set<T>(object key, Func<T> action)
        {
            var value = action();
            return _cache.Set(key, value);
        }

        public T Set<T>(object key, Func<T> action, TimeSpan absoluteExpirationRelativeToNow)
        {
            var value = action();
            return _cache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public async Task<T> SetAsync<T>(object key, Func<Task<T>> action)
        {
            var value = await action();
            return _cache.Set(key, value);
        }

        public async Task<T> SetAsync<T>(object key, Func<Task<T>> action, TimeSpan absoluteExpirationRelativeToNow)
        {
            var value = await action();
            return _cache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public T Get<T>(object key)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;
            else
                return default;
        }

        public T GetOrCreate<T>(object key, Func<T> action) => _cache.GetOrCreate(key, m => action.Invoke());

        public T GetOrCreate<T>(object key, Func<T> action, TimeSpan absoluteExpirationRelativeToNow)
        {
            var value = Get<T>(key);
            return value == default ? Set(key, action, absoluteExpirationRelativeToNow) : value;
        }

        public Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> action) => _cache.GetOrCreateAsync(key, m => action.Invoke());

        public Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> action, TimeSpan absoluteExpirationRelativeToNow)
        {
            var value = Get<T>(key);
            return value == default ? SetAsync(key, action, absoluteExpirationRelativeToNow) : Task.FromResult(value);
        }

        public void Remove(object key) => _cache.Remove(key);
    }
}
