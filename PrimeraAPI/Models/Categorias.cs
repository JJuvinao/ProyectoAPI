using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class Categorias
    {
        [Key]
        public int Id_Categoria { get; set; }
        public string? Nombre { get; set; }
    }
}
