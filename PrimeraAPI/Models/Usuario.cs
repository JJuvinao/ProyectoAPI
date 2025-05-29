using System.ComponentModel.DataAnnotations;
namespace PrimeraAPI.Models
{
    public class Usuario
    {   
        [Key]
        public int Id_Usuario { get; set; }
        public string? Nombre { get; set; }
        public string? Contrasena { get; set; }
        public string? Rol { get; set; }
        public string? Correo { get; set; }
        public byte[]? Imagen { get; set; }
    }
}
