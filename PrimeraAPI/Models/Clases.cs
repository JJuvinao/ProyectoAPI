using PrimeraAPI.ObjectDto;
using System.ComponentModel.DataAnnotations;
namespace PrimeraAPI.Models
{
    public class Clases
    {
        [Key]
        public int Id_Clase { get; set; }
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public string? Codigo { get; set; }
        public bool? Estado { get; set; } = true;
        public DateTime? FechaCreacion { get; set; }
        public int? Id_Profe { get; set; }
    }
}
