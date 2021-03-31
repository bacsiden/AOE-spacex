using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserRepository _userRepository;

        private readonly TimeSpan RoleCacheDuration = TimeSpan.FromSeconds(10);

        public FWService(IRoleRepository roleRepository,
            ILeftMenuRepository leftMenuRepository,
            ICacheManager cacheManager,
            IUserFinder userFinder,
            IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _leftMenuRepository = leftMenuRepository;
            _cacheManager = cacheManager;
            _userFinder = userFinder;
            _userRepository = userRepository;
        }

        public async Task AddActionAsync(Guid roleId, string action)
        {
            await _roleRepository.AddActionAsync(roleId, action);
            var role = await _roleRepository.GetAsync(roleId);
            role.Users.ForEach(m =>
            {
                _cacheManager.Remove($"{Constant.Cache.UserActions}{m.Id}");
            });
        }

        public async Task AddUserToRoleAsync(Guid roleId, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            await _roleRepository.AddUserAsync(roleId, new UserMeta
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Email = user.Email,
                Name = user.Name
            });
        }

        public async Task<bool> CurrentUserHasActionAsync(string action)
        {
            var userId = _userFinder.GetCurrentUserId();
            var actions = await _cacheManager.GetOrCreateAsync(Constant.Cache.UserActions + userId, () => GetUserActionsAsync(userId), RoleCacheDuration);
            return actions.Contains(action);
        }

        public async Task<Dictionary<string, List<KeyValuePair<string, bool>>>> GetRoleActionsTableAsync(Guid roleId, Func<Dictionary<string, List<KeyValuePair<string, bool>>>> getAcctions)
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

        public Task<List<LeftMenu>> GetLeftMenuForCurrentUserAsync()
        {
            var userId = _userFinder.GetCurrentUserId();
            return _cacheManager.GetOrCreateAsync($"{Constant.Cache.UserMenus}{userId}", async () =>
            {
                var actions = await GetUserActionsAsync(userId);
                var menus = await _leftMenuRepository.FindAsync(null);
                var result = menus.Where(m => m.Actions.Any(x => actions.Contains(x)) || m.Children.Any(x => x.Actions.Any(a => actions.Contains(a)))).ToList();
                foreach (var item in result)
                {
                    item.Children = item.Children.Where(m => m.Actions.Any(x => actions.Contains(x))).ToList();
                }
                result = result.Where(m => m.Children.Any() || !string.IsNullOrWhiteSpace(m.Link)).ToList();
                return result;
            }, RoleCacheDuration);
        }

        private async Task<List<string>> GetUserActionsAsync(string userId)
        {
            var roles = await _roleRepository.FindAsync(new string[] { $"{nameof(Role.Users)}._id", userId });
            return roles.SelectMany(m => m.Actions).ToList();
        }
    }
}
