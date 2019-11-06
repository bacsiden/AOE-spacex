using System.Threading.Tasks;
using AOE.Application.Base.Cache;
using AOE.Application.Models.Framework;
using AOE.Application.Repositories;

namespace AOE.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheManager _cache;
        public UserService(IUserRepository userRepository, ICacheManager cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public Task AddRoleToUserAsync(string userId, string role)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> GetAsync(string id) => _cache.GetOrCreateAsync<ApplicationUser>($"users:{id}", () => _userRepository.GetAsync(id));

        public Task RemoveRoleInUserAsync(string userId, string role)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> UpdateAsync(ApplicationUser model)
        {
            throw new System.NotImplementedException();
        }
    }
}
