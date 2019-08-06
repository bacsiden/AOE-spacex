using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AOE.Web.Models;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AOE.Web.Controllers
{
    public class HomeController : Controller
    {
        public static dynamic Data = null;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public static dynamic GetAboutUs()
        {
            using (StreamReader r = new StreamReader("about-us.json", encoding: System.Text.Encoding.UTF8))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json);
            }
        }

        public static dynamic GetContent()
        {
            //if (Data != null) return Data;
            using (StreamReader r = new StreamReader("content.json", encoding: System.Text.Encoding.UTF8))
            {
                var json = r.ReadToEnd();
                Data = JObject.Parse(json);
                return Data;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
