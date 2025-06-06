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
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IAuthService authService, IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
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

            // Configurar cookies JWT
            CookieHelper.SetAuthCookies(Response, result.Tokens!);

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

            // Configurar cookies JWT
            CookieHelper.SetAuthCookies(Response, result.Tokens!);

            return Ok(new { message = result.Message, user = result.User });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Refresh token no encontrado" });
            }

            var result = await _refreshTokenService.RefreshTokenAsync(refreshToken);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            // Configurar nuevas cookies
            CookieHelper.SetAuthCookies(Response, result.Tokens!);

            return Ok(new { message = result.Message, user = result.User });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
            }

            CookieHelper.DeleteAuthCookies(Response);
            return Ok(new { message = AuthConstants.Messages.LogoutSuccessful });
        }

        [HttpPost("logout-all")]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);
            await _refreshTokenService.RevokeAllUserRefreshTokensAsync(userId);

            CookieHelper.DeleteAuthCookies(Response);
            return Ok(new { message = "Sesión cerrada en todos los dispositivos" });
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