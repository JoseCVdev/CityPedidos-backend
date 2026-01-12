using CityPedidos.Application.DTOs.Auth;
using CityPedidos.Application.DTOs.Utilitario;
using CityPedidos.Application.Interfaces.Auth;
using CityPedidos.Infrastructure.Auth;
using CityPedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CityPedidos.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly PedidosDbContext _context;
        private readonly JwtTokenService _jwt;

        public AuthService(PedidosDbContext context, JwtTokenService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public ApiResponse<LoginResponseDto> Login(LoginRequestDto request)
        {
            var user = _context.Usuarios
                        .Include(u => u.Rol)
                            .ThenInclude(r => r.RolMenus)
                                .ThenInclude(rm => rm.menu)
                        .FirstOrDefault(x => x.vCorreo == request.email && x.bitEstado);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.vContrasena))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            var token = _jwt.GenerateToken(user.nIdUsuario, user.Rol.vNombre);

            // Mapear menú
            var menus = user.Rol.RolMenus
                .Where(x => x.menu.bitEstado)
                .Select(x => new MenuDto
                {
                    idMenu = x.menu.nIdMenu,
                    nombre = x.menu.vNombre,
                    icono = x.menu.vIcono,
                    url = x.menu.vUrl
                })
                .ToList();

            // Mapear usuario
            var usuarioDto = new UsuarioDto
            {
                idUsuario = user.nIdUsuario,
                correo = user.vCorreo,
                nombreUsuario = user.vNombreUsuario,
                descripcionRol = user.Rol.vDescripcion,
                nombreRol = user.Rol.vNombre
            };

            return ApiResponse<LoginResponseDto>.Success(new LoginResponseDto
            {
                token = token,
                expiresIn = _jwt.ExpiresInSeconds,
                menus = menus,
                usuario = usuarioDto
            });
        }


        public ApiResponse<RefreshTokenResponseDto> RefreshToken(ClaimsPrincipal userClaims)
        {
            var idUsuario = long.Parse(userClaims.FindFirst("idUsuario")!.Value);

            var usuario = _context.Usuarios
                .Include(u => u.Rol)
                    .ThenInclude(r => r.RolMenus)
                        .ThenInclude(rm => rm.menu)
                .FirstOrDefault(u => u.nIdUsuario == idUsuario && u.bitEstado);

            if (usuario == null)
                throw new UnauthorizedAccessException();

            var newToken = _jwt.GenerateToken(usuario.nIdUsuario, usuario.Rol.vNombre);

            var menus = usuario.Rol.RolMenus
                .Where(x => x.menu.bitEstado)
                .Select(x => new MenuDto
                {
                    idMenu = x.menu.nIdMenu,
                    nombre = x.menu.vNombre,
                    icono = x.menu.vIcono,
                    url = x.menu.vUrl
                })
                .ToList();

            var usuarioDto = new UsuarioDto
            {
                idUsuario = usuario.nIdUsuario,
                correo = usuario.vCorreo,
                nombreUsuario = usuario.vNombreUsuario,
                descripcionRol = usuario.Rol.vDescripcion,
                nombreRol = usuario.Rol.vNombre
            };

            return ApiResponse<RefreshTokenResponseDto>.Success(new RefreshTokenResponseDto
            {
                token = newToken,
                expiresIn = _jwt.ExpiresInSeconds,
                menus = menus,
                usuario = usuarioDto
            });
        }


    }

}
