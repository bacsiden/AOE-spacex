using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOE.Application.Base.Cache;
using AOE.Application.Base.Database;
using AOE.Application.Base.Models;

namespace AOE.Application.Base.Services
{
    public class FWService : IFWService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILeftMenuRepository _leftMenuRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IUserFinder _userFinder;

        public FWService(IRoleRepository roleRepository,
            ILeftMenuRepository leftMenuRepository,
            ICacheManager cacheManager,
            IUserFinder userFinder)
        {
            _roleRepository = roleRepository;
            _leftMenuRepository = leftMenuRepository;
            _cacheManager = cacheManager;
            _userFinder = userFinder;
        }

        public async Task AddActionAsync(Guid roleId, string action)
        {
            await _roleRepository.AddActionAsync(roleId, action);
            var role = await _roleRepository.GetAsync(roleId);
            role.Users.ForEach(m =>
            {
                _cacheManager.Remove($"{Constant.Cache.UserActions}-{m.Id}");
            });
        }

        public Task AddUserToRoleAsync(Guid roleId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Role> DeleteRoleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, List<KeyValuePair<string, bool>>>> GetActionsAsync(Guid roleId, Func<Dictionary<string, List<KeyValuePair<string, bool>>>> getAcctions)
        {
            var role = await _roleRepository.GetAsync(roleId);
            var actions = getAcctions();
            foreach (var roles in actions.Values)
            {
                for (int i = 0; i < roles.Count; i++)
                {
                    if (role.Actions.Contains(roles[i].Key)) roles[i] = new KeyValuePair<string, bool>(roles[i].Key, true);
                }
            }
            return actions;
        }

        public async Task<List<LeftMenu>> GetCurrentLeftMenuAsync()
        {
            var userId = _userFinder.GetCurrentUserId();
            var actions = await GetUserActionsAsync(userId);
            var menus = await _leftMenuRepository.FindAsync(null);
            return menus.Where(m => m.Actions.Intersect(actions).Any()).ToList();
        }

        public Task RemoveActionAsync(Guid roleId, string action)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserInRoleAsync(Guid roleId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Role> UpsertRoleAsync(Role model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetUserActionsAsync(string userId)
        {
            var roles = await _roleRepository.FindAsync(new string[] { $"{nameof(Role.Users)}._id", userId });
            return roles.SelectMany(m => m.Actions).ToList();
        }
    }
}
