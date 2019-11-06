using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AOE.App.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult GoBack(string returnUrl, string viewName, string controller = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(viewName, controller);
        }
    }
}