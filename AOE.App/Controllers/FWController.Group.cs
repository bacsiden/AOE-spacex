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
        public IActionResult Groups()
        {
            var lst = _roleRepository.GetList(m => true);
            return View(lst);
        }

        public async Task<IActionResult> GroupEdit(Guid? id)
        {
            var obj = id.HasValue ? await _roleRepository.GetAsync(id.Value) : new Role();
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> GroupEdit(Role model)
        {
            model = await _fWService.UpsertRoleAsync(model);
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteGroup(Guid groupId)
        {
            await _fWService.DeleteRoleAsync(groupId);
            return RedirectToAction(nameof(Groups));
        }

        public async Task<IActionResult> RoleGroup(Guid groupId) => View(await _fWService.GetActionsAsync(groupId, Actions.GetValues));
            //var group = await _groupRepository.GetAsync(groupId);
            //var model = Role.GetValues();
            //foreach (var roles in model.Values)
            //{
            //    for (int i = 0; i < roles.Count; i++)
            //    {
            //        if (group.Roles.Contains(roles[i].Key)) roles[i] = new KeyValuePair<string, bool>(roles[i].Key, true);
            //    }
            //}

    }
}