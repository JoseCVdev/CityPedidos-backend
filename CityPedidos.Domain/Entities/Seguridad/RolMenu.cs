using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Seguridad
{
    public class RolMenu
    {
        [Key]
        public long NIdRol { get; set; }
        public Rol rol { get; set; } = null!;

        public long nIdMenu { get; set; }
        public Menu menu { get; set; } = null!;
    }
}
