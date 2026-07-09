using AILogistics.Application.DTOs;
using AILogistics.Application.DTOs.Authentication;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AILogistics.Api.Controllers
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
