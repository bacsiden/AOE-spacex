using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AOE.Ebvie.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IEmailSender _emailSender;
        public HomeController()
        {
            //_emailSender = emailSender;
        }

        public IActionResult Index() => View();
        public IActionResult InBoard() => View();
        public IActionResult OutBoard() => View();

        public IActionResult AboutUs() => View();

        public IActionResult OutTeam() => View();

        public IActionResult Investor() => View();

        public IActionResult FAQ() => View();
        public IActionResult News() => View();
        public IActionResult Pressroom() => View();
        public IActionResult Events() => View();
        public IActionResult Contact() => View();

        [HttpPost]
        public async Task<IActionResult> Contact(string name, string email, string subject, string message)
        {
            //await _emailSender.SendEmailAsync("info@taesco.com", $"[{email} - {name}] {subject}", message);
            return Redirect("/Home/Contact?thanks=1");
        }

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

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
