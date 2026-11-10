using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
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

        [HttpPost("rejestracja")]
        public async Task<IActionResult> Rejestracja([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Rejestracja zakończona sukcesem." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}