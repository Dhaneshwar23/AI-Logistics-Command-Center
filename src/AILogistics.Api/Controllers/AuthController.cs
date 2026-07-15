using AILogistics.Api.Extensions;
using AILogistics.Application.DTOs;
using AILogistics.Application.DTOs.Authentication;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting(
    RateLimitingExtensions.AuthenticationPolicy)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            else
            {
                await _authService.RegisterAsync(request);
                return Ok("Registration successful");
            }
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            LoginResponseDto res = await _authService.LoginAsync(request);
            return Ok(res);
        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            else
            {
                LoginResponseDto res = await _authService.RefreshTokenAsync(request);
                return Ok(res);
            }
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto request)
        {
            if(request == null)
            {
                return BadRequest();
            }
            else
            {
                await _authService.LogoutAsync(request);
                return Ok("logged out successfully");
            }
        }
    }
}
