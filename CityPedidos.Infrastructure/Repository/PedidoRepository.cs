using CityPedidos.Domain.Entities.Core;
using CityPedidos.Domain.Entities.Interfaces;
using CityPedidos.Infrastructure.Data;
using CityPedidos.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;

namespace CityPedidos.Infrastructure.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosDbContext _context;
        private readonly IResilienceExecutor _resilience;

        public PedidoRepository(PedidosDbContext context, IResilienceExecutor resilience)
        {
            _context = context;
            _resilience = resilience;
        }

        public async Task<Pedido?> obtenerPedidoId(long id)
        {
            return await _resilience.ExecuteAsync(() =>
                _context.Pedidos
                    .FirstOrDefaultAsync(p => p.nIdPedido == id && p.bitEstado == true)
            );
        }

        public async Task<IEnumerable<Pedido>> obtenerPedidos()
        {
            return await _resilience.ExecuteAsync(() =>
                _context.Pedidos
                    .Where(p => p.bitEstado)
                    .OrderByDescending(p => p.nIdPedido)
                    .Include(p => p.Cliente)
                    .ToListAsync()
            );
        }

        public async Task registrarPedido(Pedido pedido)
        {
            await _resilience.ExecuteAsync(async () =>
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
            });
        }

        public async Task actualizarPedido(Pedido pedido)
        {
            await _resilience.ExecuteAsync(async () =>
            {
                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
            });
        }

        public async Task eliminarPedido(long id)
        {
            await _resilience.ExecuteAsync(async () =>
            {
                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido != null)
                {
                    pedido.bitEstado = false;
                    await _context.SaveChangesAsync();
                }
            });
        }

        public async Task<long> obtenerPedidoCorrelativo()
        {
            return await _resilience.ExecuteAsync(async () =>
            {
                var ultimoPedido = await _context.Pedidos
                    .OrderByDescending(p => p.nIdPedido)
                    .Select(p => p.vNumeroPedido)
                    .FirstOrDefaultAsync();

                return ultimoPedido == null
                    ? 0
                    : int.Parse(ultimoPedido.Replace("PED-", ""));
            });
        }
    }
}
