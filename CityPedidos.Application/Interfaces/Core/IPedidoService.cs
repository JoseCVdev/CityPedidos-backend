using CityPedidos.Application.DTOs.Auth;
using CityPedidos.Application.DTOs.Core;
using CityPedidos.Application.DTOs.Utilitario;

namespace CityPedidos.Application.Interfaces.Core
{
    public interface IPedidoService
    {
        Task<ApiResponse<PedidoRegistrarResponseDto>> RegistrarPedido(PedidoRegistrarRequestDto request);
        Task<ApiResponse<List<PedidoListResponseDto>>> ObtenerPedidos();
        Task<ApiResponse<PedidoRegistrarResponseDto>> ActualizarPedido(long id, PedidoRegistrarRequestDto request);
        Task<ApiResponse<bool>> EliminarPedido(long idPedido);
    }
}
