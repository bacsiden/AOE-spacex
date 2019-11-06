using System.Threading.Tasks;
using AOE.Application.Base.Services;
using AOE.Application.Base.Database;
using Microsoft.AspNetCore.Mvc;
using AOE.Application.Repositories;
using AOE.Application.Services;

namespace AOE.App.Controllers
{
    public partial class FWController : BaseController
    {
        private readonly IFWService _fWService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public FWController(IFWService fWService,
            IRoleRepository groupRepository,
            IUserRepository userRepository,
            IUserService userService)
        {
            _fWService = fWService;
            _roleRepository = groupRepository;
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<IActionResult> LeftMenu()
        {
            var menus = await _fWService.GetCurrentLeftMenuAsync();
            return View(menus);
        }

    }
}