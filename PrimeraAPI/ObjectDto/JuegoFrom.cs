namespace PrimeraAPI.ObjectDto
{
    public class JuegoFrom
    {
        public string? Nombre { get; set; }
        public string? Genero { get; set; }
        public string? Tema { get; set; }
    }

    public class Preg_HeroesDto
    {
        public string? Pregunta { get; set; }
        public string? RespuestaV { get; set; }
        public string? RespuestaF1 { get; set; }
        public string? RespuestaF2 { get; set; }
        public string? RespuestaF3 { get; set; }
    }

    public class AhorcadoDto
    {
        public string? Palabra { get; set; }
        public string? Pista { get; set; }
    }
}
