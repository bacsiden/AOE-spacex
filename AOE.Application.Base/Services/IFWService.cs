using AOE.Application.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AOE.Application.Base.Services
{
    public interface IFWService
    {
        Task<List<LeftMenu>> GetLeftMenuForCurrentUserAsync();

        Task AddUserToRoleAsync(Guid roleId, string userId);

        Task<Dictionary<string, List<KeyValuePair<string, bool>>>> GetRoleActionsTableAsync(Guid roleId, Func<Dictionary<string, List<KeyValuePair<string, bool>>>> getAcctions);

        Task<bool> CurrentUserHasActionAsync(string action);
    }
}
