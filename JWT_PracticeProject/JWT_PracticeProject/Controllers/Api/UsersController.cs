using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWT_PracticeProject.Models;
using JWT_PracticeProject.Repositories;

namespace JWT_PracticeProject.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QueryUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (newId, message) = await _userRepository.CreateUserAsync(user);
            if (newId <= 0)
            {
                return BadRequest(new { success = false, message });
            }

            user.UserId = newId;
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { success = true, message, user });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] QueryUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.UserId = id;
            var (success, message) = await _userRepository.UpdateUserAsync(user);
            if (!success)
            {
                return BadRequest(new { success = false, message });
            }

            return Ok(new { success = true, message, user });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _userRepository.DeleteUserAsync(id);
            if (!success)
            {
                return BadRequest(new { success = false, message });
            }

            return Ok(new { success = true, message });
        }
    }
}
