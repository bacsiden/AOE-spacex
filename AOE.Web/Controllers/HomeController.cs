using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AOE.Web.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace AOE.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult AboutUs() => View();

        public IActionResult RepaireOverhual() => View();

        public IActionResult Services() => View();

        public IActionResult Certifications() => View();

        public IActionResult Contact() => View();

        public static string lang = "-en";
        public IActionResult ChangeLanguage(string url)
        {
            lang = lang == "-vn" ? "-en" : "-vn";
            return RedirectToAction(url);
        }

        public static dynamic GetData(HttpRequest request, string dataName)
        {
            var path = $"data\\{dataName}{lang}.json";
            using (StreamReader r = new StreamReader(path, encoding: System.Text.Encoding.UTF8))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
