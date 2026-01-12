namespace CityPedidos.Application.DTOs.Core
{
    public class PedidoRegistrarRequestDto
    {
        public DateTime fechaPedido { get; set; }
        public decimal total { get; set; }
        public long idCliente { get; set; }
    }
}
