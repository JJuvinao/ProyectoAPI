namespace PrimeraAPI.Models
{
    public class Clases
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tema { get; set; }
        public string Autor { get; set; }
        public string Codigo { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
