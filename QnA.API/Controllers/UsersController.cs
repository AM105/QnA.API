using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QnA.API.Models;
using QnA.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace QnA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found", id);
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _logger.LogInformation("Creating a new user: {Username}", user.Username);
            var createdUser = await _userService.CreateUserAsync(user);
            _logger.LogInformation("User created with ID: {UserId}", createdUser.Id);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                _logger.LogWarning("Bad request: ID mismatch. Path ID: {PathId}, User ID: {UserId}", id, user.Id);
                return BadRequest();
            }

            // Check if the user is updating their own profile
            var userId = User.FindFirst("sub")?.Value;
            if (userId != id.ToString())
            {
                _logger.LogWarning("User {AuthUserId} attempted to update user {TargetUserId}", userId, id);
                return Forbid();
            }

            _logger.LogInformation("User {UserId} updating their profile", id);
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Check if the user is deleting their own profile
            var userId = User.FindFirst("sub")?.Value;
            if (userId != id.ToString())
            {
                _logger.LogWarning("User {AuthUserId} attempted to delete user {TargetUserId}", userId, id);
                return Forbid();
            }

            _logger.LogInformation("User {UserId} deleting their profile", id);
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}

