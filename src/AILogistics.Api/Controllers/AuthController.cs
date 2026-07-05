using AILogistics.Application.DTOs.Authentication;
using AILogistics.Application.Interfaces;
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
    }
}
