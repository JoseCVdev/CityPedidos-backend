using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Exceptions
{
    public class ErrorResponseDto
    {
        public bool ok { get; set; } = false;
        public int status { get; set; }
        public string error { get; set; } = string.Empty;
    }
}
