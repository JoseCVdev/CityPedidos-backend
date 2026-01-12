using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Seguridad
{
    public class Menu
    {
        [Key]
        public long nIdMenu { get; set; }
        public string vNombre { get; set; } = null!;
        public string vIcono { get; set; } = null!;
        public string vUrl { get; set; } = null!;
        public bool bitEstado { get; set; }

        public ICollection<RolMenu> RolMenus { get; set; } = new List<RolMenu>();
    }
}
