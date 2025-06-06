using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using server.Models.DTOs;
using server.Services.Interfaces;
using server.Helpers;
using server.Constants;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            // Configurar cookie JWT
            CookieHelper.SetJwtCookie(Response, result.Token!);

            return Ok(new { message = result.Message, user = result.User });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            // Configurar cookie JWT
            CookieHelper.SetJwtCookie(Response, result.Token!);

            return Ok(new { message = result.Message, user = result.User });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            CookieHelper.DeleteJwtCookie(Response);
            return Ok(new { message = AuthConstants.Messages.LogoutSuccessful });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _authService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
} 