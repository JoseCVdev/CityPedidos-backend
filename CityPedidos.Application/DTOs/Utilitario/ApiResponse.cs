using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Application.DTOs.Utilitario
{
    public class ApiResponse<T>
    {
        public bool Ok { get; set; }
        public int Status { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        public static ApiResponse<T> Success(T data, int status = 200) => new ApiResponse<T>
        {
            Ok = true,
            Status = status,
            Data = data
        };

        public static ApiResponse<T> Fail(string error, int status) => new ApiResponse<T>
        {
            Ok = false,
            Status = status,
            Error = error
        };
    }
}
