
using PrimeraAPI.Models;

namespace PrimeraAPI.ObjectDto
{
    public class ExamenDto
    {
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenExamen { get; set;}
        public int? Id_Clase { get; set; }
        public int? Id_Juego { get; set; }
        public List<Preg_HeroesDto>? Heroes { get; set; }
        public string? Palabra { get; set; }
        public string? Pista { get; set; }
    }

    public class Examen_HeroesDto
    {
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenExamen { get; set; }
        public int? Id_Clase { get; set; }
        public int? Id_Juego { get; set; }
        public List<Preg_HeroesDto>? Heroes { get; set; }
    }

    public class Examen_AhorcadoDto
    {
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenExamen { get; set; }
        public int? Id_Clase { get; set; }
        public int? Id_Juego { get; set; }
        public string? Palabra { get; set; }
        public string? Pista { get; set; }
    }

    public class ExamenGet
    {
        public int Id_Examen { get; set; }
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public string? Descripcion { get; set; }
        public string? Codigo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? ImagenExamen { get; set; }
        public int? Id_Juego { get; set; }
    }
}
