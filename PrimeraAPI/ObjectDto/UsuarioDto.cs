namespace PrimeraAPI.ObjectDto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
        public string? Correo { get; set; }
        public string? Imagen { get; set; }
        public bool? Premium { get; set; }
    }

    public class UsuarioShow
    {
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
        public string? Imagen { get; set; }
    }
    public class UserDelete
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
    }

    public class UsuarioFrom
    {
        public string? Nombre { get; set; }
        public string? Contrasena { get; set; }
        public string? Rol { get; set; }
        public string? Correo { get; set; }
        public string? Imagen { get; set; }
    }
}
