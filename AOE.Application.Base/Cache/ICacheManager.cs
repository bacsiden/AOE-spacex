using System;
using System.Threading.Tasks;

namespace AOE.Application.Base.Cache
{
    public interface ICacheManager
    {
        T Get<T>(object key);
        T GetOrCreate<T>(object key, Func<T> action);
        T GetOrCreate<T>(object key, Func<T> action, TimeSpan absoluteExpirationRelativeToNow);
        Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> action);
        Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> action, TimeSpan absoluteExpirationRelativeToNow);
        void Remove(object key);
        T Set<T>(object key, Func<T> action);
        T Set<T>(object key, Func<T> action, TimeSpan absoluteExpirationRelativeToNow);
        T Set<T>(object key, T value);
        T Set<T>(object key, T value, TimeSpan absoluteExpirationRelativeToNow);
        Task<T> SetAsync<T>(object key, Func<Task<T>> action);
        Task<T> SetAsync<T>(object key, Func<Task<T>> action, TimeSpan absoluteExpirationRelativeToNow);
    }
}