using CityPedidos.Domain.Entities.Core;
using CityPedidos.Domain.Entities.Interfaces;
using CityPedidos.Infrastructure.Data;
using CityPedidos.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;

namespace CityPedidos.Infrastructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly PedidosDbContext _context;
        private readonly IResilienceExecutor _resilience;

        public ClienteRepository(
            PedidosDbContext context,
            IResilienceExecutor resilience
        )
        {
            _context = context;
            _resilience = resilience;
        }

        public async Task<bool> existeCliente(long idCliente)
        {
            return await _resilience.ExecuteAsync(() =>
                _context.Clientes.AnyAsync(c =>
                    c.nIdCliente == idCliente && c.bitEstado
                )
            );
        }

        public async Task<Cliente?> obtenerPorNumeroDocumento(string numeroDocumento)
        {
            return await _resilience.ExecuteAsync(() =>
                _context.Clientes
                    .FirstOrDefaultAsync(c => c.vNumeroDocumento == numeroDocumento)
            );
        }

        public async Task<Cliente?> obtenerClientePorId(long idCliente)
        {
            return await _resilience.ExecuteAsync(() =>
                _context.Clientes
                    .FirstOrDefaultAsync(c =>
                        c.nIdCliente == idCliente && c.bitEstado
                    )
            );
        }

        public async Task registrar(Cliente cliente)
        {
            await _resilience.ExecuteAsync(async () =>
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            });
        }

        public async Task actualizar(Cliente cliente)
        {
            await _resilience.ExecuteAsync(async () =>
            {
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            });
        }

        public async Task<IEnumerable<Cliente>> obtenerClientes()
        {
            return await _resilience.ExecuteAsync(() =>
                _context.Clientes
                    .Where(c => c.bitEstado)
                    .OrderByDescending(c => c.nIdCliente)
                    .ToListAsync()
            );
        }
    }
}
