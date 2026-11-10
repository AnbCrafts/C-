using JWT_PracticeProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JWT_PracticeProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Index", "UsersView");
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
