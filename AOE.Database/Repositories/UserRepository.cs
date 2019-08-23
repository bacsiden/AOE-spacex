using AOE.Application.Base.Database;
using AOE.Application.Models.Framework;
using AOE.Application.Repositories;
using MongoDB.Driver;

namespace AOE.Database.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(IMongoDatabase db) : base(db, "Users") { }
    }
}
