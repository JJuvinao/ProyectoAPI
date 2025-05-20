using System.ComponentModel.DataAnnotations;

namespace PrimeraAPI.Models
{
    public class Productos
    {
        [Key]
        public int Id_Producto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Precio { get; set; }
        public int Id_Categoria { get; set; }
    }
}
