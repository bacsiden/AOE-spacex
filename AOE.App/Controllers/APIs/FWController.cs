using AOE.Application.Base.Database;
using AOE.Application.Base.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers.APIs
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public partial class FWController : ControllerBase
    {
        private readonly IFWService _fWService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public FWController(IFWService fWService,
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        {
            _fWService = fWService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
    }
}
