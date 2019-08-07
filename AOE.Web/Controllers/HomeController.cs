using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AOE.Web.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace AOE.Web.Controllers
{
    public class HomeController : Controller
    {
        public static class DataName
        {
            public const string Content = "content";
            public const string AboutUs = "about-us";
            public const string Service = "services";
        }
        public static dynamic Data = null;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult RepaireOverhual()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult QualityAssurance()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ChangeLanguage()
        {
            var lang = string.Empty;
            Request.Cookies.TryGetValue("lang", out lang);
            Response.Cookies.Delete("lang");
            if (lang == "en")
                Response.Cookies.Append("lang", "vn");
            else
                Response.Cookies.Append("lang", "en");
            return RedirectToAction(nameof(Index));
        }

        public static dynamic GetAboutUs()
        {
            using (StreamReader r = new StreamReader("data\\about-us.json", encoding: System.Text.Encoding.UTF8))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json);
            }
        }
        
        public static dynamic GetServices()
        {
            using (StreamReader r = new StreamReader("data\\services.json", encoding: System.Text.Encoding.UTF8))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json);
            }
        }

        public static dynamic GetContent()
        {
            //if (Data != null) return Data;
            using (StreamReader r = new StreamReader("data\\content.json", encoding: System.Text.Encoding.UTF8))
            {
                var json = r.ReadToEnd();
                Data = JObject.Parse(json);
                return Data;
            }
        }

        public static dynamic GetData(HttpRequest request, string dataName)
        {
            var lang = string.Empty;
            request.Cookies.TryGetValue("lang", out lang);
            lang = lang == "vn" ? "-vn" : null;
            var path = $"data\\{dataName}{lang}.json";
            using (StreamReader r = new StreamReader(path, encoding: System.Text.Encoding.UTF8))
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
