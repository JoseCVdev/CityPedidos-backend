using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Core
{
    public class ClienteResponseDto
    {
        public long idCliente { get; set; }
        public string numeroDocumento { get; set; } = string.Empty;
        public string nombres { get; set; } = string.Empty;
        public string apellidoPaterno { get; set; } = string.Empty;
        public string apellidoMaterno { get; set; } = string.Empty;
        public bool estado { get; set; }
    }
}
