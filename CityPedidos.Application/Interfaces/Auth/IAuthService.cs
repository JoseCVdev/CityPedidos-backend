using CityPedidos.Application.DTOs.Auth;
using CityPedidos.Application.DTOs.Utilitario;
using System.Security.Claims;

namespace CityPedidos.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        ApiResponse<LoginResponseDto> Login(LoginRequestDto request);
        ApiResponse<RefreshTokenResponseDto> RefreshToken(ClaimsPrincipal userClaims);
    }
}
