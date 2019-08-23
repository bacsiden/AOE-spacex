using AOE.Application.Models.Framework;
using System.Threading.Tasks;

namespace AOE.Application.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetAsync(string id);
    }
}
