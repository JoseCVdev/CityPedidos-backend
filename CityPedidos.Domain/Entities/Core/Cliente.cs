using CityPedidos.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Core
{
    public class Cliente : AuditableEntity
    {
        [Key]
        public long nIdCliente { get; set; }
        public string vNumeroDocumento { get; set; } = null!;
        public string vNombres { get; set; } = null!;
        public string vApellidoPaterno { get; set; } = null!;
        public string vApellidoMaterno { get; set; } = null!;
        public bool bitEstado { get; set; }
    }
}
