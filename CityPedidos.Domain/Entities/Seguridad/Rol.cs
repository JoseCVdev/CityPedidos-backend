using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Seguridad
{
    public class Rol
    {
        [Key]
        public long nIdRol { get; set; }
        public string vNombre { get; set; } = null!;
        public string vDescripcion { get; set; } = null!;
        public bool bitEstado { get; set; }

        public ICollection<RolMenu> RolMenus { get; set; } = new List<RolMenu>();
    }
}
