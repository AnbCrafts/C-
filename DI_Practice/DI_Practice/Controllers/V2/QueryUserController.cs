using Microsoft.AspNetCore.Mvc;
using DI_Practice.Models;
using DI_Practice.Services;

namespace DI_Practice.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
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
            var rawUsers = _userService.GetAllQueries();

            var transformedData = rawUsers.Select(u => new QueryUserV2Dto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                FullName = u.FullName,
                Gender = u.Gender,
                IsActive = u.IsActive,
                Contact = new ContactDetails
                {
                    Email = u.Email,
                    MobileNo = u.MobileNo
                },
                Location = new LocationDetails
                {
                    City = u.City,
                    StateName = u.StateName,
                    Country = u.Country
                },
                Query = new QueryDetails
                {
                    Text = u.QueryText,
                    Amount = u.QueryAmount,
                    CreatedDate = u.CreatedDate
                }
            }).ToList();

            var response = new QueryUserV2Response
            {
                Status = "Success",
                StatusCode = 200,
                ApiVersion = "2.0",
                GeneratedAt = DateTime.UtcNow,
                TotalCount = transformedData.Count,
                Data = transformedData
            };

            return Ok(response);
        }
    }
}
