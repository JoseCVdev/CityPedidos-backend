using CityPedidos.Application.DTOs.Auth;
using CityPedidos.Application.DTOs.Core;
using CityPedidos.Application.DTOs.Utilitario;
using CityPedidos.Application.Exceptions;
using CityPedidos.Application.Interfaces.Core;
using CityPedidos.Domain.Entities.Core;
using CityPedidos.Domain.Entities.Interfaces;

namespace CityPedidos.Application.Services.Core
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IClienteRepository _clienteRepo;

        public PedidoService(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo)
        {
            _pedidoRepo = pedidoRepo;
            _clienteRepo = clienteRepo;
        }

        public async Task<ApiResponse<PedidoRegistrarResponseDto>> RegistrarPedido(PedidoRegistrarRequestDto request)
        {
            // Validar que el total sea mayor a 0
            if (request.total <= 0) throw new BadRequestException("El total debe ser mayor a 0");

            // Validando existencia del cliente
            var cliente = await _clienteRepo.obtenerClientePorId(request.idCliente);
            if (cliente == null) throw new BadRequestException("Cliente no encontrado");

            // Armando el correlativo
            var ultimoNumero = await _pedidoRepo.obtenerPedidoCorrelativo();
            var nuevoNumero = $"PED-{(ultimoNumero + 1).ToString("D3")}";

            var pedido = new Pedido
            {
                vNumeroPedido = nuevoNumero,
                decTotal = request.total,
                dtFechaPedido = request.fechaPedido,
                nIdCliente = request.idCliente,
                bitEstado = true
            };

            await _pedidoRepo.registrarPedido(pedido);

            return ApiResponse<PedidoRegistrarResponseDto>.Success(new PedidoRegistrarResponseDto
            {
                numeroPedido = nuevoNumero
            });
        }

        public async Task<ApiResponse<List<PedidoListResponseDto>>> ObtenerPedidos()
        {
            var pedidos = await _pedidoRepo.obtenerPedidos();

            var response = pedidos.Select(p => new PedidoListResponseDto
            {
                idPedido = p.nIdPedido,
                numeroPedido = p.vNumeroPedido,
                fechaPedido = p.dtFechaPedido,
                total = p.decTotal,
                estado = p.bitEstado,
                cliente = new ClienteResponseDto
                {
                    idCliente = p.Cliente.nIdCliente,
                    numeroDocumento = p.Cliente.vNumeroDocumento,
                    nombres = p.Cliente.vNombres,
                    apellidoPaterno = p.Cliente.vApellidoPaterno,
                    apellidoMaterno = p.Cliente.vApellidoMaterno
                }
            }).ToList();

            return ApiResponse<List<PedidoListResponseDto>>.Success(response);
        }

        public async Task<ApiResponse<PedidoRegistrarResponseDto>> ActualizarPedido(long id, PedidoRegistrarRequestDto request)
        {
            var pedido = await _pedidoRepo.obtenerPedidoId(id);

            if (pedido == null) throw new BadRequestException("Pedido no encontrado");

            if (request.total <= 0) throw new BadRequestException("El total debe ser mayor a 0");

            var cliente = await _clienteRepo.obtenerClientePorId(request.idCliente);
            if (cliente == null) throw new BadRequestException("Cliente no encontrado");

            pedido.decTotal = request.total;
            pedido.dtFechaPedido = request.fechaPedido;
            pedido.nIdCliente = request.idCliente;

            await _pedidoRepo.actualizarPedido(pedido);

            return ApiResponse<PedidoRegistrarResponseDto>.Success(new PedidoRegistrarResponseDto
            {
                numeroPedido = pedido.vNumeroPedido
            });
        }

        public async Task<ApiResponse<bool>> EliminarPedido(long id)
        {
            var pedido = await _pedidoRepo.obtenerPedidoId(id);

            if (pedido == null) throw new BadRequestException("Pedido no encontrado");

            await _pedidoRepo.eliminarPedido(id);

            return ApiResponse<bool>.Success(true);
        }
    }
}
