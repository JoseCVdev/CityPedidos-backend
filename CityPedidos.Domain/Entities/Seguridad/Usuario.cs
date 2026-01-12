using CityPedidos.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Seguridad
{
    public class Usuario : AuditableEntity
    {
        [Key]
        public long nIdUsuario { get; set; }
        public string vCorreo { get; set; } = null!;
        public string vContrasena { get; set; } = null!;
        public string vNombreUsuario { get; set; } = null!;
        public bool bitEstado { get; set; }
        public long nIdRol { get; set; }
        public Rol Rol { get; set; } = null!;
    }
}
