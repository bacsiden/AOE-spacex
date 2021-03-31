using AOE.Application.Base.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace AOE.App.Services
{
    public class UserFinder : IUserFinder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserFinder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
