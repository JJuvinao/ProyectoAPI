using System.ComponentModel.DataAnnotations;
namespace PrimeraAPI.Models
{
    public class Examenes
    {
        [Key]
        public int Id_Examen { get; set; }
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public string? Descripcion { get; set; }
        public string? Codigo { get; set; }
        public int? Tiempo { get; set; }
        public bool Privacidad { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime? FechaCreacion { get; set; }
        public int? Id_Clase { get; set; }
        public int? Id_Juego { get; set; }
    }
}
