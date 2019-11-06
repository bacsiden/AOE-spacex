using AOE.Application.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AOE.Application.Base.Services
{
    public interface IFWService
    {
        Task<List<LeftMenu>> GetCurrentLeftMenuAsync();

        Task<Role> UpsertRoleAsync(Role model);
        Task<Role> DeleteRoleAsync(Guid id);
        Task AddUserToRoleAsync(Guid roleId, string userId);
        Task RemoveUserInRoleAsync(Guid roleId, string userId);

        Task<Dictionary<string, List<KeyValuePair<string, bool>>>> GetActionsAsync(Guid roleId, Func<Dictionary<string, List<KeyValuePair<string, bool>>>> getAcctions);
        Task AddActionAsync(Guid roleId, string action);
        Task RemoveActionAsync(Guid roleId, string action);
    }
}
