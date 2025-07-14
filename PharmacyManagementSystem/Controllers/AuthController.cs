using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.DTOS.Authentication;
using PharmacyManagementSystem.Interfaces;

namespace PharmacyManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                await _authRepository.RegisterAsync(dto);
                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var response = await _authRepository.LoginAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

    }
}
