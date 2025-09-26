using System.ComponentModel.DataAnnotations; 

namespace PrimeraAPI.Models
{
    public class Palabras_Ahorcado
    {
        [Key]
        public int Id { get; set; }
        public string? Palabra { get; set; }
        public string? Pista { get; set; }
        public string? Codigo_Exa { get; set; }
    }
}
