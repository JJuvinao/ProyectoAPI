namespace PrimeraAPI.Models
{
    public class Estudi_Examen
    {
        public int Id { get; set; }
        public int? Id_Estudiane { get; set; }
        public int? Id_Examen { get; set; }
        public int? Intentos { get; set; }
        public int? Aciertos { get; set; }
        public int? Fallos { get; set; }
        public float? Nota { get; set; }
        public string? Recomendacion { get; set; }
    }
}
