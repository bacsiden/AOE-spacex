using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOE.Application.Base.Models;
using AOE.Application.Models.Framework;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers.APIs
{
    [Route("api/[controller]")]
    public partial class FWController : ControllerBase
    {
        [HttpGet("roles/{id}")]
        public Task GetRole(Guid id) => _roleRepository.GetAsync(id);

        [HttpGet("roles")]
        public IList<Role> GetRole() => _roleRepository.GetList(m => true);

        [HttpDelete("roles/{id}")]
        public Task DeleteRole(Guid roleId) => _fWService.DeleteRoleAsync(roleId);

        [HttpPost("roles/users/{id}")]
        public Task AddUserToRole(Guid roleId, string userId) => _fWService.AddUserToRoleAsync(roleId, userId);

        [HttpDelete("roles/users/{id}")]
        public Task RemoveUserInRole(Guid roleId, string userId) => _fWService.RemoveUserInRoleAsync(roleId, userId);

        [HttpPost("roles/actions/{id}")]
        public Task AddAction(Guid roleId, string action) => _fWService.AddActionAsync(roleId, action);

        [HttpDelete("roles/actions/{id}")]
        public Task RemoveAction(Guid roleId, string action) => _fWService.RemoveActionAsync(roleId, action);

        [HttpGet("actions")]
        public Task<Dictionary<string, List<KeyValuePair<string, bool>>>> GetActions(Guid roleId) => _fWService.GetActionsAsync(roleId, Actions.GetValues);
    }
}
