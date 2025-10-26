using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class Cursos
    {
        [Key]
        public int Id_curso { get; set; }
        public string? Json_string { get; set; }
        public int Num_sections { get; set; }
        public int Percentage { get; set; }
        public bool Completed { get; set; }
        public int Id_user { get; set; }
        public string? Codigo_Curso { get; set; }
        }
}
