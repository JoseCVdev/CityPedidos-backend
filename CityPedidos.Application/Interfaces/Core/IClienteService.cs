using CityPedidos.Application.DTOs.Core;
using CityPedidos.Application.DTOs.Utilitario;

namespace CityPedidos.Application.Interfaces.Core
{
    public interface IClienteService
    {
        Task<ApiResponse<ClienteResponseDto>> RegistrarCliente(ClienteRegistrarRequestDto request);
        Task<ApiResponse<bool>> EliminarCliente(long idCliente);
        Task<ApiResponse<List<ClienteListResponseDto>>> ObtenerClientes();
        Task<ApiResponse<ClienteResponseDto>> ObtenerClientePorDocumento(string numeroDocumento);
    }
}
