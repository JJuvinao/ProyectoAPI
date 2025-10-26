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
            var cursos_user = cursos.Where(c => c.Id_user == Id_user).ToList();
            if (cursos_user != null)
            {
                return Ok(_CursosDeserializer(cursos));
            }

            var Cursosflitardos = new List<Cursos>();
            List<int> IdCursosList = Cursoslist(Id_user);
            foreach (var id in IdCursosList)
            {
                var curso = cursos.FirstOrDefault(c => c.Id_curso == id);
                if (curso != null)
                {
                    Cursosflitardos.Add(curso);
                }
            }

            if (Cursosflitardos.Count == 0)
            {
                return NotFound("No hay cursos");
            }
            return Ok(_CursosDeserializer(Cursosflitardos));
        }

        private List<int> Cursoslist(int id)
        {
            var IdCurso = _context.User_Cursos.Where(e => e.Id_user == id).ToList();
            var IdCursoList = new List<int>();
            foreach (var item in IdCurso)
            {
                if (item.Id_curso != null)
                {
                    IdCursoList.Add((int)item.Id_curso);
                }
            }
            return IdCursoList;
        }

        [HttpPost("AIGenerate")]
        public async Task<IActionResult> PostCurso(Request request)
        {
            string codigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
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
                Id_user = Id_user,
                Codigo_Curso = codigo
            };

            _context.Cursos.Add(cursodb);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Ingresar_Curso")]
        public async Task<IActionResult> Ingresar_Curso(User_Cursodto usercurso)
        {
            var cursoExistente = await _context.Cursos.FirstOrDefaultAsync(c => c.Codigo_Curso == usercurso.Codigo);
            if (cursoExistente == null)
            {
                return NotFound("Código de curso inválido");
            }

            var user_curso = new User_Cursos
            {
                Id_curso = cursoExistente.Id_curso,
                Id_user = usercurso.Id_user
            };
            
            _context.User_Cursos.Add(user_curso);
            await _context.SaveChangesAsync();
            return Ok("Curso ingresado correctamente");
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
                cursoDto.Codigo = curso.Codigo_Curso;
                cursosDto.Add(cursoDto);
            }
            return cursosDto;
        }
    }
}