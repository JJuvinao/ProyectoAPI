using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class User_Cursos
    {
        [Key]
        public int Id_User_Curso { get; set; }
        public int Id_user { get; set; }
        public int Id_curso { get; set; }
    }
}
