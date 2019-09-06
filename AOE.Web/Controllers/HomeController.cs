using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AOE.Web.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;

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

        public IActionResult ChangeLanguage()
        {
            Request.Cookies.TryGetValue("lang", out string lang);
            Response.Cookies.Delete("lang");
            if (lang == "vn")
                Response.Cookies.Append("lang", "en");
            else
                Response.Cookies.Append("lang", "vn");
            return RedirectToAction(nameof(Index));
        }

        public static dynamic GetData(HttpRequest request, string dataName)
        {
            request.Cookies.TryGetValue("lang", out string lang);
            lang = lang == "vn" ? "-vn" : null;
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
