using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AOE.Application.Base.Database
{
    public interface IBaseRepository<T> where T : class
    {
        IMongoCollection<T> _collection { get; }

        T Add(T model);
        Task<T> AddAsync(T model);
        void AddRange(IEnumerable<T> list);
        Task AddRangeAsync(IEnumerable<T> list);
        void Delete(object id);
        Task DeleteAsync(object id);
        void DeleteMany(string fieldName, object value);
        Task DeleteManyAsync(string fieldName, object value);
        IList<T> GetList(Expression<Func<T, bool>> predicate);
        IMongoQueryable<T> Find(Expression<Func<T, bool>> predicate);
        IFindFluent<T, T> Find(IEnumerable<object> ids);
        IList<T> Find(object[] pars, int skip = 0, int take = -1);
        T Get(object id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(object id);
        T Save(Guid id, Action<UpdateOperations<T>> updates);
        T Update(T model);
        Task<T> UpdateAsync(T model);
        T Upsert(T model);
        Task<T> UpsertAsync(T model);
    }
}