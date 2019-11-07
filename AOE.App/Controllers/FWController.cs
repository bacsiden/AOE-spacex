using System.Threading.Tasks;
using AOE.Application.Base.Services;
using AOE.Application.Base.Database;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers
{
    public partial class FWController : BaseController
    {
        private readonly IFWService _fWService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public FWController(IFWService fWService,
            IRoleRepository groupRepository,
            IUserRepository userRepository)
        {
            _fWService = fWService;
            _roleRepository = groupRepository;
            _userRepository = userRepository;
        }

        [ResponseCache(Duration = 120)]
        public async Task<IActionResult> LeftMenu()
        {
            var menus = await _fWService.GetLeftMenuForCurrentUserAsync();
            return View(menus);
        }
    }
}