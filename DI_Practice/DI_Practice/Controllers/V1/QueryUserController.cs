using Microsoft.AspNetCore.Mvc;
using DI_Practice.Services;

namespace DI_Practice.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Route("[controller]")] // backward compatibility: /queryuser also routes to v1
    public class QueryUserController : ControllerBase
    {
        private readonly IQueryUserService _userService;

        public QueryUserController(IQueryUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _userService.GetAllQueries();
            return Ok(data);
        }
    }
}
