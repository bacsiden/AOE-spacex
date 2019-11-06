using AOE.Application.Base.Database;
using AOE.Application.Base.Services;
using AOE.Application.Repositories;
using AOE.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class FWController : ControllerBase
    {
        private readonly IFWService _fWService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public FWController(IFWService fWService,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUserService userService)
        {
            _fWService = fWService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userService = userService;
        }
    }
}
