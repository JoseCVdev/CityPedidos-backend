using CityPedidos.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Core
{
    public class Pedido : AuditableEntity
    {
        [Key]
        public long nIdPedido { get; set; }
        public string vNumeroPedido { get; set; } = null!;
        public DateTime dtFechaPedido { get; set; }
        public decimal decTotal { get; set; }
        public bool bitEstado { get; set; }

        public long nIdCliente { get; set; }
        public Cliente Cliente { get; set; } = null!;
    }
}
