using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_PracticeProject_2.Data;
using WebAPI_PracticeProject_2.DTOs;
using WebAPI_PracticeProject_2.Models;

namespace WebAPI_PracticeProject_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QueryUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/QueryUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QueryUser>>> GetQueryUsers(
            [FromQuery] string? search = null,
            [FromQuery] string? city = null,
            [FromQuery] bool? isActive = null)
        {
            var query = _context.QueryUsers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(search) ||
                    u.Email.Contains(search) ||
                    (u.FullName != null && u.FullName.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(u => u.City != null && u.City.ToLower() == city.ToLower());
            }

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            return await query.OrderByDescending(u => u.UserId).ToListAsync();
        }

        // GET: api/QueryUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QueryUser>> GetQueryUser(int id)
        {
            var queryUser = await _context.QueryUsers.FindAsync(id);

            if (queryUser == null)
            {
                return NotFound(new { message = $"QueryUser with UserId {id} not found." });
            }

            return queryUser;
        }

        // POST: api/QueryUsers
        [HttpPost]
        public async Task<ActionResult<QueryUser>> CreateQueryUser([FromBody] CreateQueryUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var queryUser = new QueryUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
                MobileNo = dto.MobileNo,
                City = dto.City,
                StateName = dto.StateName,
                Country = dto.Country,
                Gender = dto.Gender,
                QueryText = dto.QueryText,
                CreatedDate = DateTime.Now,
                IsActive = dto.IsActive ?? true,
                QueryAmount = dto.QueryAmount
            };

            _context.QueryUsers.Add(queryUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQueryUser), new { id = queryUser.UserId }, queryUser);
        }

        // PUT: api/QueryUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQueryUser(int id, [FromBody] UpdateQueryUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var queryUser = await _context.QueryUsers.FindAsync(id);

            if (queryUser == null)
            {
                return NotFound(new { message = $"QueryUser with UserId {id} not found." });
            }

            queryUser.UserName = dto.UserName;
            queryUser.Email = dto.Email;
            queryUser.FullName = dto.FullName;
            queryUser.MobileNo = dto.MobileNo;
            queryUser.City = dto.City;
            queryUser.StateName = dto.StateName;
            queryUser.Country = dto.Country;
            queryUser.Gender = dto.Gender;
            queryUser.QueryText = dto.QueryText;
            queryUser.IsActive = dto.IsActive;
            queryUser.QueryAmount = dto.QueryAmount;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await QueryUserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(queryUser);
        }

        // DELETE: api/QueryUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQueryUser(int id)
        {
            var queryUser = await _context.QueryUsers.FindAsync(id);
            if (queryUser == null)
            {
                return NotFound(new { message = $"QueryUser with UserId {id} not found." });
            }

            _context.QueryUsers.Remove(queryUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"QueryUser with UserId {id} deleted successfully." });
        }

        private async Task<bool> QueryUserExists(int id)
        {
            return await _context.QueryUsers.AnyAsync(e => e.UserId == id);
        }
    }
}
