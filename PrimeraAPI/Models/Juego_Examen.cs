using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class Juego_Examen
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre_Juego { get; set; }
        public string? Codigo_Exa { get; set; }
    }
}
