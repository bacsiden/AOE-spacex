using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AOE.Application.Base.Models;
using Microsoft.AspNetCore.Mvc;
using AOE.Application.Models.Framework;

namespace AOE.App.Controllers
{
    public partial class FWController : BaseController
    {
        public IActionResult Roles()
        {
            var lst = _roleRepository.GetList(m => true);
            return View(lst);
        }

        public async Task<IActionResult> RoleEdit(Guid? id)
        {
            var obj = id.HasValue ? await _roleRepository.GetAsync(id.Value) : new Role();
            return View(obj);
        }

        [HttpPost]
        public IActionResult RoleEdit(Role model)
        {
            _roleRepository.Save(model.Id, m => m.Set(x => x.Title, model.Title));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(Guid groupId)
        {
            await _roleRepository.DeleteAsync(groupId);
            return RedirectToAction(nameof(Roles));
        }

        public async Task<IActionResult> RoleActions(Guid groupId) => View(await _fWService.GetRoleActionsTableAsync(groupId, Actions.GetValues));
    }
}