using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PrimeraAPI.Controllers
{
    [Route("api/Cursos")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ContextDB _context;

        public CursosController(ContextDB context)
        {
            _context = context;
        }

        [HttpGet("{Id_user}")]
        public async Task<ActionResult<IEnumerable<CursoDto>>> Get(int Id_user)
        {
            var cursos = await _context.Cursos.ToListAsync();
            if (cursos == null)
            {
                return NotFound("No hay cursos");
            }

            return Ok(_CursosDeserializer(cursos));
        }

        [HttpPost("AIGenerate")]
        public async Task<IActionResult> PostCurso(Request request)
        {
            int Id_user = request.Id_user;
            string userRequest = request.userRequest;

            AICourseController aICourseController = new AICourseController(new HttpClient());
            var cursoJson = await aICourseController.GenerateCourseJson(userRequest);
            var curso = JsonSerializer.Deserialize<CursoDto>(cursoJson);
            int numSections = _CalculateSections(curso.modules, curso.questions.Count);
            var cursodb = new Cursos
            {
                Json_string = cursoJson,
                Num_sections = numSections,
                Percentage = 0,
                Completed = false,
                Id_user = Id_user
            };

            _context.Cursos.Add(cursodb);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{Id_course}")]
        public async Task<IActionResult> DeleteCourse(int Id_course)
        {
            var curso = await _context.Cursos.FindAsync(Id_course);
            if (curso == null)
            {
                return NotFound("Usuario no existe");
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return Ok("Curso eliminado");
        }

        [HttpPut]
        public async Task<ActionResult> PutCourse(CursoDto cursoDto)
        {
            var curso = await _context.Cursos.FindAsync(cursoDto.Id_curso);
            if (curso == null)
            {
                return NotFound();
            }

            string newJson = JsonSerializer.Serialize(cursoDto);

            curso.Json_string = newJson;
            curso.Completed = cursoDto.Completed;
            curso.Percentage = cursoDto.Percentage;


            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound("Fallo la modificacion en base de datos");
            }

            return Ok("Modificación guardada");
        }

        private int _CalculateSections(List<Modulo> modules, int count)
        {
            int sections = 0;
            foreach (var module in modules)
            {
                sections = sections + module.lessons.Count;
            }

            return sections + count;
        }

        private List<CursoDto> _CursosDeserializer(List<Cursos> cursos)
        {
            var cursosDto = new List<CursoDto>();
            foreach (var curso in cursos)
            {
                var cursoDto = JsonSerializer.Deserialize<CursoDto>(curso.Json_string);
                cursoDto.Id_curso = curso.Id_curso;
                cursoDto.Num_sections = curso.Num_sections;
                cursoDto.Percentage = curso.Percentage;
                cursoDto.Completed = curso.Completed;
                cursoDto.Id_user = curso.Id_user;
                cursosDto.Add(cursoDto);
            }
            return cursosDto;
        }
    }
}