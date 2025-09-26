using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class Preguntas_Heroes
    {
        [Key]
        public int Id { get; set; }
        public string? Pregunta { get; set; }
        public string? RespuestaV { get; set; }
        public string? RespuestaF1 { get; set; }
        public string? RespuestaF2 { get; set; }
        public string? RespuestaF3 { get; set; }
        public string? Codigo_Exa { get; set; }
    }
}
