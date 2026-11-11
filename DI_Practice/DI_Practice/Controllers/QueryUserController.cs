using Microsoft.AspNetCore.Mvc;
using DI_Practice.Services;

namespace DI_Practice.Controllers
{
    public class QueryUserController : Controller
    {
        private readonly IQueryUserService _userService;

        // Constructor Injection: ASP.NET Core DI provides the instance automatically
        public QueryUserController(IQueryUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = _userService.GetAllQueries();
            return Json(data);
        }
    }
}
