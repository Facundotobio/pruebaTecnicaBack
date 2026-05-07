using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaFacundoTobioBack.Application.DTOs;
using PruebaTecnicaFacundoTobioBack.Application.Services;

namespace PruebaTecnicaFacundoTobioBack.Presentation.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            if (response == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas o cliente no activo" });
            }
            return Ok(response);
        }
    }
}
