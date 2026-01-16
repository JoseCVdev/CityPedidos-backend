using CityPedidos.Application.DTOs.Core;
using CityPedidos.Application.DTOs.Utilitario;
using CityPedidos.Application.Exceptions;
using CityPedidos.Application.Interfaces.Core;
using CityPedidos.Domain.Entities.Core;
using CityPedidos.Domain.Entities.Interfaces;
using System.Text.RegularExpressions;

namespace CityPedidos.Application.Services.Core
{
    public class ClienteService : IClienteService
    {

        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<ApiResponse<ClienteResponseDto>> RegistrarCliente(ClienteRegistrarRequestDto request)
        {
            // Validar numero documento
            if (!Regex.IsMatch(request.numeroDocumento, @"^\d{8}$"))
                throw new BadRequestException("El número de documento debe tener 8 dígitos y solo números");

            // Buscar cliente existente
            var clienteExistente = await _clienteRepo.obtenerPorNumeroDocumento(request.numeroDocumento);

            if (clienteExistente != null)
            {
                // Si existe pero está inactivo → activarlo
                if (!clienteExistente.bitEstado)
                {
                    clienteExistente.vNombres = request.nombres;
                    clienteExistente.vApellidoPaterno = request.apellidoPaterno;
                    clienteExistente.vApellidoMaterno = request.apellidoMaterno;
                    clienteExistente.bitEstado = true;
                    await _clienteRepo.actualizar(clienteExistente);

                    return ApiResponse<ClienteResponseDto>.Success(new ClienteResponseDto
                    {
                        idCliente = clienteExistente.nIdCliente,
                        numeroDocumento = clienteExistente.vNumeroDocumento,
                        nombres = clienteExistente.vNombres,
                        apellidoPaterno = clienteExistente.vApellidoPaterno,
                        apellidoMaterno = clienteExistente.vApellidoMaterno,
                        estado = clienteExistente.bitEstado
                    });
                } else
                {
                    throw new BadRequestException($"El cliente con el documento {request.numeroDocumento} ya existe.");
                }             
            }

            // Registrar nuevo cliente
            var cliente = new Cliente
            {
                vNumeroDocumento = request.numeroDocumento,
                vNombres = request.nombres,
                vApellidoPaterno = request.apellidoPaterno,
                vApellidoMaterno = request.apellidoMaterno,
                bitEstado = true
            };

            await _clienteRepo.registrar(cliente);

            return ApiResponse<ClienteResponseDto>.Success(new ClienteResponseDto
            {
                idCliente = cliente.nIdCliente,
                numeroDocumento = cliente.vNumeroDocumento,
                nombres = cliente.vNombres,
                apellidoPaterno = cliente.vApellidoPaterno,
                apellidoMaterno = cliente.vApellidoMaterno,
                estado = cliente.bitEstado
            });
        }

        public async Task<ApiResponse<bool>> EliminarCliente(long idCliente)
        {
            var cliente = await _clienteRepo.obtenerClientePorId(idCliente);

            if (cliente == null) throw new BadRequestException("Cliente no encontrado");

            cliente.bitEstado = false;
            await _clienteRepo.actualizar(cliente);

            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<List<ClienteListResponseDto>>> ObtenerClientes()
        {
            var clientes = await _clienteRepo.obtenerClientes();

            var response = clientes.Select(p => new ClienteListResponseDto
            {
                idCliente = p.nIdCliente,
                numeroDocumento = p.vNumeroDocumento,
                nombres = p.vNombres,
                apellidoPaterno = p.vApellidoPaterno,
                apellidoMaterno = p.vApellidoMaterno
            }).ToList();

            return ApiResponse<List<ClienteListResponseDto>>.Success(response);
        }

        public async Task<ApiResponse<ClienteResponseDto>> ObtenerClientePorDocumento(string numeroDocumento)
        {
            var cliente = await _clienteRepo.obtenerPorNumeroDocumento(numeroDocumento);

            if (cliente == null || !cliente.bitEstado) throw new BadRequestException("Cliente no encontrado");

            return ApiResponse<ClienteResponseDto>.Success(
                new ClienteResponseDto
                {
                    idCliente = cliente.nIdCliente,
                    numeroDocumento = cliente.vNumeroDocumento,
                    nombres = cliente.vNombres,
                    apellidoPaterno = cliente.vApellidoPaterno,
                    apellidoMaterno = cliente.vApellidoMaterno,
                    estado = cliente.bitEstado
                }
            );
        }
    }
}
