using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRedirect.Extension;
using Sample.Models;

namespace Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy(string title, string title2)
        {
            ViewBag.Title = title;
            ViewBag.Title2 = title2;
            return View();
        }

        public IActionResult Another()
        {
            var valueFromField = "Cool value";
            return this.RedirectTo<HomeController>(a => a.Privacy(valueFromField, "Cool value 2"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}