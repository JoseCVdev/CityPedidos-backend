using CityPedidos.Application.DTOs.Core;
using CityPedidos.Application.DTOs.Utilitario;
using CityPedidos.Application.Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CityPedidos.Controllers.Pedidos
{
    [EnableRateLimiting("fixed")]
    [Authorize]
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ApiResponse<List<PedidoListResponseDto>>>> ListarPedidos()
        {
            var result = await _pedidoService.ObtenerPedidos();
            return Ok(result);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<ApiResponse<PedidoRegistrarResponseDto>>> RegistrarPedido([FromBody] PedidoRegistrarRequestDto request)
        {
            var result = await _pedidoService.RegistrarPedido(request);
            return Ok(result);
        }


        [HttpPut("actualizar/{idPedido}")]
        public async Task<IActionResult> ActualizarPedido(long idPedido, [FromBody] PedidoRegistrarRequestDto request)
        {
            var result = await _pedidoService.ActualizarPedido(idPedido, request);
            return Ok(result);
        }

        [HttpDelete("eliminar/{idPedido}")]
        [Authorize(Roles = "ADMIN_ROLE")]
        public async Task<IActionResult> Eliminardido(long idPedido)
        {
            var result = await _pedidoService.EliminarPedido(idPedido);
            return Ok(result);
        }
    }
}
