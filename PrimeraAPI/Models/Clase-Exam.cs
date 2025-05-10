namespace PrimeraAPI.Models
{
    public class Clase_Exam
    {
        public int Id { get; set; }
        public int id_clase { get; set; }
        public int id_examen { get; set; }
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
    }
}
