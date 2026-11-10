using Microsoft.AspNetCore.Mvc;

namespace JWT_PracticeProject.Controllers
{
    [Route("Users")]
    public class UsersViewController : Controller
    {
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View("~/Views/Users/Index.cshtml");
        }
    }
}
