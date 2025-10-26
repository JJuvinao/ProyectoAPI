using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace PrimeraAPI.Controllers
{
    public class AICourseController
    {
        private readonly HttpClient _httpClient;
        public AICourseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(300);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");
        }

        public async Task<string> GenerateCourseJson(string userRequest)
        {
            var requestBody = new
            {
                model = "deepseek/deepseek-chat-v3.1:free",
                messages = new[]
            {
                new
                {
                    role = "system",
                    content = "Eres un asistente que genera contenido educativo. Responde siempre en formato JSON siguiendo esta estructura estricta: \n" +
                              "{\n" +
                              "  \"title\": \"string\",\n" +
                              "  \"description\": \"string\",\n" +
                              "  \"modules\": [\n" +
                              "    {\n" +
                              "      \"id\": 0,\n" +
                              "      \"title\": \"string\",\n" +
                              "      \"description\": \"string\",\n" +
                              "      \"lessons\": [\n" +
                              "        {\n" +
                              "          \"id\": 0,\n" +
                              "          \"title\": \"string\",\n" +
                              "          \"content\": \"string\"\n" +
                              "        }\n" +
                              "      ]\n" +
                              "    }\n" +
                              "  ],\n" +
                              "  \"questions\": [\n" +
                              "    {\n" +
                              "      \"id\": 0,\n" +
                              "      \"text\": \"string\",\n" +
                              "      \"options\": [\"string\", \"string\", \"string\", \"string\"],\n" +
                              "      \"answerIndex\": 0,\n" +
                              "      \"response\": \"string\"\n" +
                              "    }\n" +
                              "  ]\n" +
                              "}\n" +
                              "Genera un curso educativo con todos los modulos posibles con todas las lecciones necesarias." +
                              " Incluye todas las preguntas necesarias que esten relacionadas con las lecciones, " +
                              "y en el Response el texto que explique la respuesta correcta"
                },
                new
                {
                    role = "user",
                    content = $"Genera un curso sobre {userRequest}"
                }
            }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
            var result = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(result);
            var contentJson = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            int startIndex = contentJson.IndexOf("{");
            var endIndex = contentJson.LastIndexOf("}");
            return contentJson.Substring(startIndex, endIndex - startIndex + 1);
        }
    }
}
