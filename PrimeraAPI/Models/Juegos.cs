using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class Juegos
    {
        [Key]
        public int Id_Juego { get; set; }
        public string? Nombre { get; set; }
        public string? Genero { get; set; }
        public string? Tema { get; set; }
        public bool? Estado { get; set; }

    }
}
