using AOE.Application.Base.Services;
using System;
using System.Security.Claims;

namespace AOE.App.Services
{
    public class UserFinder : IUserFinder
    {
        public string GetCurrentUserId()
        {
            return ClaimsPrincipal.Current?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
