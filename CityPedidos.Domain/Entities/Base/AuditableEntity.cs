using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Base
{
    public class AuditableEntity
    {
        public long nIdUsuarioCrea { get; set; }
        public DateTime dtFechaCrea { get; set; }

        public long? nIdUsuarioMod { get; set; } = null!;
        public DateTime? dtFechaMod { get; set; }

        public long? ndUsuarioEli { get; set; } = null!;
        public DateTime? dtFechaEli { get; set; }
    }
}
