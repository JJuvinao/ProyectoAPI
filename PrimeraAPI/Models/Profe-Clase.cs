namespace PrimeraAPI.Models
{
    public class Profe_Clase
    {
        public int id { get; set; }
        public int id_profesor { get; set; }
        public int id_clase { get; set; }
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
    }
}
