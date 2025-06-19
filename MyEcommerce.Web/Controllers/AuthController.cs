using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEcommerce.Core.DTOs;
using MyEcommerce.Core.Entities;
using MyEcommerce.Core.Interface;

namespace MyEcommerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new UserEntity
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role
            };

            var result = await _authService.RegisterAsync(user, dto.Password);
            if (result == null)
                return BadRequest("Email already exists");

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            var user = await _authService.LoginAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _authService.GenerateToken(user);
            return Ok(new { token, user.Id, user.Email, user.FullName });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var success = await _authService.ChangePasswordAsync(dto.UserId, dto.CurrentPassword, dto.NewPassword);
            return success ? Ok("Password changed") : BadRequest("Invalid current password");
        }

        [Authorize]
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> Profile(int userId)
        {
            var user = await _authService.GetProfileAsync(userId);
            return user != null ? Ok(user) : NotFound();
        }
    }
}
