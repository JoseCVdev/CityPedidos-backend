using CityPedidos.Application.DTOs.Auth;
using CityPedidos.Application.DTOs.Utilitario;
using CityPedidos.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityPedidos.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<ApiResponse<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var result = _authService.Login(request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public ActionResult<ApiResponse<RefreshTokenResponseDto>> RefreshToken()
        {
            var result = _authService.RefreshToken(User);
            return Ok(result);
        }
    }
}
