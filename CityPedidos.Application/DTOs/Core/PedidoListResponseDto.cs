using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Core
{
    public class PedidoListResponseDto
    {
        public long idPedido { get; set; }
        public string numeroPedido { get; set; } = null!;
        public DateTime fechaPedido { get; set; }
        public decimal total { get; set; }
        public bool estado { get; set; }

        public ClienteResponseDto cliente { get; set; } = null!;
    }
}
