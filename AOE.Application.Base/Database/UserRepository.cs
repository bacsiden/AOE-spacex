using AOE.Application.Base.Database;
using AOE.Application.Base.Models;
using MongoDB.Driver;

namespace AOE.Application.Base.Database
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(IMongoDatabase db) : base(db, "Users") { }
    }
}
