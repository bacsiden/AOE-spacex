using System;
using System.Threading.Tasks;

namespace AOE.Application.Base.Cache
{
    public interface ICacheManager
    {
        void Create(object key, object value);

        T Get<T>(object key);

        T GetOrCreate<T>(object key, Func<T> action);

        Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> action);

        void Remove(object key);
    }
}
