using Microsoft.AspNetCore.Mvc;
using QnA.API.DTOs;
using QnA.API.Services.Interfaces;
using Serilog;

namespace QnA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", registerDto.Username);
            
            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
            {
                _logger.LogWarning("Registration failed for user: {Username}. Reason: {Message}", 
                    registerDto.Username, result.Message);
                return BadRequest(result);
            }
            
            _logger.LogInformation("Registration successful for user: {Username}", registerDto.Username);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);
            
            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
            {
                _logger.LogWarning("Login failed for user: {Username}. Reason: {Message}", 
                    loginDto.Username, result.Message);
                return Unauthorized(result);
            }
            
            _logger.LogInformation("Login successful for user: {Username}", loginDto.Username);
            return Ok(result);
        }
    }
}

