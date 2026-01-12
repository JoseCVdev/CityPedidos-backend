namespace CityPedidos.Application.DTOs.Auth
{
    public class MenuDto
    {
        public long idMenu { get; set; }
        public string nombre { get; set; } = null!;
        public string icono { get; set; } = null!;
        public string url { get; set; } = null!;
    }
}
