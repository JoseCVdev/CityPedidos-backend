using CityPedidos.Application.DTOs.Core;
using CityPedidos.Application.DTOs.Utilitario;
using CityPedidos.Application.Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityPedidos.Controllers.Pedidos
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> RegistrarCliente([FromBody] ClienteRegistrarRequestDto request)
        {
            var result = await _clienteService.RegistrarCliente(request);
            return Ok(result);
        }

        [HttpDelete("eliminar/{idCliente}")]
        [Authorize(Roles = "ADMIN_ROLE")]
        public async Task<ActionResult<ApiResponse<bool>>> EliminarCliente(long idCliente)
        {
            var result = await _clienteService.EliminarCliente(idCliente);
            return Ok(result);
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ApiResponse<ClienteListResponseDto>>> ListarClientes()
        {
            var result = await _clienteService.ObtenerClientes();
            return Ok(result);
        }

        [HttpGet("obtener-por-documento/{numeroDocumento}")]
        public async Task<ActionResult<ApiResponse<ClienteListResponseDto>>> ObtenerClientePorDocumento(string numeroDocumento)
        {
            var result = await _clienteService.ObtenerClientePorDocumento(numeroDocumento);
            return Ok(result);
        }

    }
}
