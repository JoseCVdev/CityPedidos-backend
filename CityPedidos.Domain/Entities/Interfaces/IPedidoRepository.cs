using CityPedidos.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPedidos.Domain.Entities.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> obtenerPedidoId(long id);
        Task<IEnumerable<Pedido>> obtenerPedidos();
        Task registrarPedido(Pedido pedido);
        Task actualizarPedido(Pedido pedido);
        Task eliminarPedido(long id);
        Task<long> obtenerPedidoCorrelativo();
    }
}
