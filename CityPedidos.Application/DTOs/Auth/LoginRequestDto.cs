using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
