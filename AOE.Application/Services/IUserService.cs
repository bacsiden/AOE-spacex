using AOE.Application.Models.Framework;
using System.Threading.Tasks;

namespace AOE.Application.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetAsync(string id);
        Task<ApplicationUser> UpdateAsync(ApplicationUser model);
        Task AddRoleToUserAsync(string userId, string role);
        Task RemoveRoleInUserAsync(string userId, string role);
    }
}
