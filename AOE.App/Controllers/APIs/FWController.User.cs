using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers.APIs
{
    public partial class FWController : ControllerBase
    {
        [HttpDelete]
        [Route("users/{id}")]
        public Task RemoveUser(string id) => _userRepository.DeleteAsync(id);
    }
}
