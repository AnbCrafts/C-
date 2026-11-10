using Microsoft.AspNetCore.Mvc;

namespace JWT_PracticeProject.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in via cookie, redirect to Users
            if (Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            Response.Cookies.Delete("refresh_token");
            return RedirectToAction("Login");
        }
    }
}
