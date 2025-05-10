namespace PrimeraAPI.ObjectDto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Correo { get; set; }

        public UsuarioDto() { }

        public UsuarioDto(int id, string nombre, string rol, string correo)
        {
            Id = id;
            Nombre = nombre;
            Rol = rol;
            Correo = correo;
        }
    }
}
