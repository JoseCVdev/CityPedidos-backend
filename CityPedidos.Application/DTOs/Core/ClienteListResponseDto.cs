namespace CityPedidos.Application.DTOs.Core
{
    public class ClienteListResponseDto
    {
        public long idCliente { get; set; }
        public string numeroDocumento { get; set; } = null!;
        public string nombres { get; set; } = null!;
        public string apellidoPaterno { get; set; } = null!;
        public string apellidoMaterno { get; set; } = null!;
    }
}