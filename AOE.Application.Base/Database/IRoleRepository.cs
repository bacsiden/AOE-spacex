using System;
using System.Threading.Tasks;
using AOE.Application.Base.Models;

namespace AOE.Application.Base.Database
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task AddUserAsync(Guid groupId, UserMeta user);
        Task RemoveUserAsync(Guid groupId, string userId);
        Task AddActionAsync(Guid groupId, string action);
        Task RemoveActionAsync(Guid groupId, string action);
    }
}