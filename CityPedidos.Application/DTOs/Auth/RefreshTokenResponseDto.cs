using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Auth
{
    public class RefreshTokenResponseDto
    {
        public string token { get; set; } = null!;
        public int expiresIn { get; set; }
        public List<MenuDto> menus { get; set; } = new();
        public UsuarioDto usuario { get; set; } = null!;
    }
}
