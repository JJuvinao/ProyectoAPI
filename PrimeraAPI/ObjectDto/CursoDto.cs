using System.Text.Json.Serialization;

namespace PrimeraAPI.ObjectDto
{
    public class CursoDto
    {
        [JsonIgnore]
        public int Id_curso { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<Modulo> modules { get; set; }
        public List<Pregunta> questions { get; set; }
        [JsonIgnore]
        public bool Completed { get; set; }
        [JsonIgnore]
        public int Num_sections { get; set; }
        [JsonIgnore]
        public int Percentage { get; set; }
        [JsonIgnore]
        public int Id_user { get; set; }
    }

    public class Modulo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<Leccion> lessons { get; set; }
        public bool Completed { get; set; }
    }

    public class Leccion
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public bool Completed { get; set; }
    }

    public class Pregunta
    {
        public int id { get; set; }
        public string text { get; set; }
        public string[] options { get; set; }
        public int answerIndex { get; set; }
        public string response { get; set; }
        public bool Completed { get; set; }
    }
}
