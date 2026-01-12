using CityPedidos.Domain.Entities.Core;

namespace CityPedidos.Domain.Entities.Interfaces
{
    public interface IClienteRepository
    {
        Task<bool> existeCliente(long idCliente);
        Task<Cliente?> obtenerPorNumeroDocumento(string numeroDocumento);
        Task<Cliente?> obtenerClientePorId(long idCliente);
        Task registrar(Cliente cliente);
        Task actualizar(Cliente cliente);
        Task<IEnumerable<Cliente>> obtenerClientes();
    }
}
