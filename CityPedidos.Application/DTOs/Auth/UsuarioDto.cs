using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Auth
{
    public class UsuarioDto
    {
        public long idUsuario { get; set; }
        public string correo { get; set; } = null!;
        public string nombreUsuario { get; set; } = null!;
        public string descripcionRol { get; set; } = null!;
        public string nombreRol { get; set; } = null!;
    }
}
