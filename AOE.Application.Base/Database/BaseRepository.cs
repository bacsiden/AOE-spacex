using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AOE.Application.Base.Database
{
    public abstract class BaseRepository<T> where T : class
    {
        protected IMongoCollection<T> _collection;
        public BaseRepository(IMongoDatabase database, string collectionName = null) => _collection = database.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? $"{typeof(T).Name}s" : collectionName);

        private bool IsAddNew(T model)
        {
            var id = typeof(T).GetProperty("Id").GetValue(model);
            return id is Guid ? (Guid)id == Guid.Empty ? true : false : string.IsNullOrEmpty((string)id) ? true : false;
        }

        private object ConvertId(object id) => id;

        private IEnumerable<object> ConvertIds(IEnumerable<object> ids) => ids.Select(ConvertId);

        public virtual T Get(object id) => _collection.Find(Builders<T>.Filter.Eq("_id", ConvertId(id))).FirstOrDefault();

        public virtual Task<T> GetAsync(object id) => _collection.Find(Builders<T>.Filter.Eq("_id", ConvertId(id))).FirstOrDefaultAsync();

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate) => _collection.AsQueryable().FirstOrDefaultAsync(predicate);

        public virtual IList<T> GetList(Expression<Func<T, bool>> predicate) => _collection.AsQueryable().Where(predicate).ToList();

        public virtual IMongoQueryable<T> Find(Expression<Func<T, bool>> predicate) => _collection.AsQueryable().Where(predicate);

        public virtual IFindFluent<T, T> Find(IEnumerable<object> ids) => _collection.Find(Builders<T>.Filter.In("_id", ConvertIds(ids)));
        private IFindFluent<T, T> FindFluent(object[] pars, int skip = 0, int take = -1)
        {
            if (pars == null) return take == -1 ? _collection.Find(Builders<T>.Filter.Empty).Skip(skip) : _collection.Find(Builders<T>.Filter.Empty).Skip(skip).Limit(take);
            if (pars.Length % 2 != 0) throw new ArgumentException($"{nameof(pars)} is not valid, its length must be even");

            var filter = Builders<T>.Filter.Empty;
            for (int i = 0; i < pars.Length; i += 2)
            {
                filter &= Builders<T>.Filter.Eq(pars[i].ToString(), pars[i + 1]);
            }
            return take == -1 ? _collection.Find(filter).Skip(skip) : _collection.Find(filter).Skip(skip).Limit(take);
        }
        public virtual IList<T> Find(object[] pars, int skip = 0, int take = -1) => FindFluent(pars, skip, take).ToList();
        public virtual Task<List<T>> FindAsync(object[] pars, int skip = 0, int take = -1) => FindFluent(pars, skip, take).ToListAsync();

        public virtual T Add(T model)
        {
            _collection.InsertOne(model);
            return model;
        }

        public virtual async Task<T> AddAsync(T model)
        {
            await _collection.InsertOneAsync(model);
            return model;
        }

        public virtual void AddRange(IEnumerable<T> list) => _collection.InsertMany(list);

        public virtual Task AddRangeAsync(IEnumerable<T> list) => _collection.InsertManyAsync(list);

        public virtual T Upsert(T model) => IsAddNew(model) ? Add(model) : Update(model);

        public virtual Task<T> UpsertAsync(T model) => IsAddNew(model) ? AddAsync(model) : UpdateAsync(model);

        public virtual T Update(T model)
        {
            var id = typeof(T).GetProperty("Id").GetValue(model);
            var filter_id = Builders<T>.Filter.Eq("_id", ConvertId(id));
            _collection.ReplaceOne(filter_id, model);

            return model;
        }

        public virtual async Task<T> UpdateAsync(T model)
        {
            var id = typeof(T).GetProperty("Id").GetValue(model);
            var filter_id = Builders<T>.Filter.Eq("_id", ConvertId(id));
            await _collection.ReplaceOneAsync(filter_id, model);

            return model;
        }

        public virtual T Save(Guid id, Action<UpdateOperations<T>> updates)
        {
            var operations = new UpdateOperations<T>();
            updates(operations);

            var filter_id = Builders<T>.Filter.Eq("_id", ConvertId(id));
            var result = _collection.FindOneAndUpdate<T>(
                filter: filter_id,
                update: operations.GetDefinition(),
                options: new FindOneAndUpdateOptions<T> { ReturnDocument = ReturnDocument.After }
            );

            return result;
        }

        public virtual void Delete(object id) => _collection.DeleteOne(Builders<T>.Filter.Eq("_id", ConvertId(id)));

        public virtual Task DeleteAsync(object id) => _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", ConvertId(id)));

        public virtual void DeleteMany(string fieldName, object value) => _collection.DeleteMany(Builders<T>.Filter.Eq(fieldName, value));

        public virtual Task DeleteManyAsync(string fieldName, object value) => _collection.DeleteManyAsync(Builders<T>.Filter.Eq(fieldName, value));
    }
}
