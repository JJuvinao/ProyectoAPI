namespace PrimeraAPI.ObjectDto
{
    public class Estu_ExamDto
    {
        public int? Id_Estudiane { get; set; }
        public int? Id_Examen { get; set; }
    }

    public class Estu_ExamFrom
    {
        public int? Id_Estudiane { get; set; }
        public int? Id_Examen { get; set; }

        public int? Intentos { get; set; }
        public int? Aciertos { get; set; }
        public int? Fallos { get; set; }
        public float? Notas { get; set; }
        public string? Recomendaciones { get; set; }
    }

    public class Estu_ExamPut
    {
        public int? Id_estu_exa { get; set; }
        public int? Id_estu { get; set; }
        public float? Nota { get; set; }
        public string? Recomendacion { get; set; }
    }
}
