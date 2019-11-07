using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOE.Application.Base.Models;
using AOE.Application.Models.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers.APIs
{
    public partial class FWController : ControllerBase
    {
        [HttpGet]
        [Route("roles/{id}")]
        public Task<Role> GetRole(Guid id) => _roleRepository.GetAsync(id);

        [HttpGet]
        [Route("roles")]
        public IList<Role> GetRoles() => _roleRepository.GetList(m => true);

        [HttpDelete]
        [Route("roles/{roleId}")]
        public Task DeleteRole(Guid roleId) => _roleRepository.DeleteAsync(roleId);

        [HttpPost]
        [Route("roles/{roleId}/users/{userId}")]
        public Task AddUserToRole(Guid roleId, string userId) => _fWService.AddUserToRoleAsync(roleId, userId);

        [HttpPost]
        [Route("roles/{roleId}/users/{userId}")]
        public Task RemoveUserInRole(Guid roleId, string userId) => _roleRepository.RemoveUserAsync(roleId, userId);

        [HttpPost]
        [Route("roles/{roleId}/actions/{action}")]
        public Task AddAction(Guid roleId, string action) => _roleRepository.AddActionAsync(roleId, action);

        [HttpPost]
        [Route("roles/{roleId}/actions/{action}")]
        public Task RemoveAction(Guid roleId, string action) => _roleRepository.RemoveActionAsync(roleId, action);

        [HttpGet]
        [Route("roles/{roleId}/actions")]
        public Task<Dictionary<string, List<KeyValuePair<string, bool>>>> GetActionsTable(Guid roleId) => _fWService.GetActionsAsync(roleId, Actions.GetValues);
    }
}
