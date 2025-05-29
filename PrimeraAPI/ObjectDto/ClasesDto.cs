namespace PrimeraAPI.ObjectDto
{
    public class ClasesDto
    {
        public string? Nombre { get; set; }
        public string? Tema { get; set; }
        public string? Autor { get; set; }
        public IFormFile ImagenClase { get; set; }
        public int? Id_Profe { get; set; }
    }
}
