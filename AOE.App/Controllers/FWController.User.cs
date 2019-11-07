using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AOE.Application.Base.Models;
using Microsoft.AspNetCore.Mvc;
using AOE.Application.Base;

namespace AOE.App.Controllers
{
    public partial class FWController : BaseController
    {
        public IActionResult Users(BaseFilter filter)
        {
            var lst = _userRepository.GetList(m => true);
            return View(lst);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var obj = await _userRepository.GetAsync(id);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(ApplicationUser model, string returnUrl)
        {
            await _userRepository.UpdateAsync(model);
            return GoBack(returnUrl, nameof(Users));
        }
    }
}