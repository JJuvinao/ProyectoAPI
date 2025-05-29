namespace PrimeraAPI.ObjectDto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
        public string? Correo { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
