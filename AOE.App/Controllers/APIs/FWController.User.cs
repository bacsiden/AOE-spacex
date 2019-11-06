using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers.APIs
{
    [Route("api/[controller]")]
    public partial class FWController : ControllerBase
    {
        [HttpPost]
        public Task RemoveUser(string userId) => _userRepository.DeleteAsync(userId);

        [HttpPost]
        public Task AddRoleToUser(string userId, string role) => _userService.AddRoleToUserAsync(userId: userId, role);

        [HttpPost]
        public Task RemoveRoleInUser(string userId, string role) => _userService.RemoveRoleInUserAsync(userId: userId, role);
    }
}
