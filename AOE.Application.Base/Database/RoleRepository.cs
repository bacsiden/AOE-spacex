using AOE.Application.Base.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOE.Application.Base.Database
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IMongoDatabase db) : base(db, "roles") { }

        public Task AddActionAsync(Guid groupId, string action) => _collection.UpdateOneAsync(Builders<Role>.Filter.Where(x => x.Id == groupId), Builders<Role>.Update.Push(x => x.Actions, action));

        public Task AddUserAsync(Guid groupId, UserMeta user) => _collection.UpdateOneAsync(Builders<Role>.Filter.Where(x => x.Id == groupId), Builders<Role>.Update.Push(x => x.Users, user));

        public Task RemoveActionAsync(Guid groupId, string action) => _collection.UpdateOneAsync(Builders<Role>.Filter.Where(x => x.Id == groupId), Builders<Role>.Update.Pull(x => x.Actions, action));

        public Task RemoveUserAsync(Guid groupId, string userId) => _collection.UpdateOneAsync(Builders<Role>.Filter.Where(x => x.Id == groupId), Builders<Role>.Update.Pull(x => x.Users.Select(m => m.Id), userId));
    }
}
