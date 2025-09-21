using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;
using System.Linq;

namespace PrimeraAPI.Controllers
{
    [Route("api/Estudi_Examen")]
    [ApiController]
    public class Estu_ExamController : Controller
    {
        private readonly ContextDB _context;

        public Estu_ExamController(ContextDB context)
        {
            _context = context;
        }

        // GET: Estudi_Examen/
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpPost("get_estu_exa")]
        public async Task<ActionResult<Estu_ExamFrom>> GetEstudi_Examen(Estu_ExamDto estu_ExamDto)
        {
            if (estu_ExamDto != null)
            {

                if (EstudiExists(estu_ExamDto.Id_Estudiane))
                {
                    var estu_Examen = await _context.Estudi_Examenes
                        .Where(e => e.Id_Estudiane == estu_ExamDto.Id_Estudiane
                        && e.Id_Examen == estu_ExamDto.Id_Examen)
                        .ToListAsync();

                    if (estu_Examen == null)
                    {
                        return NotFound("No hay examen disponible");
                    }
                    return Ok(estu_Examen);
                }
                return NotFound("No existe el usuario");
            }
            return NotFound();
        }

        private bool EstudiExists(int? id)
        {
            return _context.Usuarios.Any(e => e.Id_Usuario == id);
        }


        // POST: Estudi_Examen
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpPost("IngresarExa")]
        public async Task<ActionResult<Estu_ExamFrom>> PostEstudi_Clase(Estu_ExamFrom estu_Exam)
        {
            if (!EstudiExists(estu_Exam.Id_Estudiane))
                return BadRequest("El estudiante no existe");

            var examen = await _context.Examenes.FirstOrDefaultAsync(e => e.Id_Examen == estu_Exam.Id_Examen);
            if (examen == null)
                return NotFound("Codigo de clase ínvalido o inexistente");

            var estudi_exam = new Estudi_Examen
            {
                Id_Estudiane = estu_Exam.Id_Estudiane,
                Id_Examen = examen.Id_Examen,
                Intentos = estu_Exam.Intentos,
                Aciertos = estu_Exam.Aciertos,
                Fallos = estu_Exam.Fallos,
                Nota = estu_Exam.Notas,
                Recomendacion = estu_Exam.Recomendaciones,
            };

            _context.Estudi_Examenes.Add(estudi_exam);
            await _context.SaveChangesAsync();

            return Ok("Ingreso a la clase exitoso");
        }

        [Authorize(Roles = "Profesor")]
        [HttpPut("Calificar")]
        public async Task<ActionResult> PutCalificar(Estu_ExamPut estu_Exam)
        {
            var RelacionEstudiExam = await _context.Estudi_Examenes
                .FirstOrDefaultAsync(e => e.Id_Estudiane == estu_Exam.Id_estu && e.Id == estu_Exam.Id_estu_exa);
            if (RelacionEstudiExam == null)
            {
                return NotFound();
            }

            RelacionEstudiExam.Nota = estu_Exam.Nota;
            RelacionEstudiExam.Recomendacion = estu_Exam.Recomendacion;


            _context.Entry(RelacionEstudiExam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound("Fallo la modificacion en base de datos");
            }

            return Ok("Calificación guardada");
        }

        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpGet("UsersExamen/{id_exam}")]
        public async Task<ActionResult<UsuarioDto>> GetUsersclase(int id_exam)
        {
            if (ExamenExiste(id_exam))
            {
                var users = await _context.Usuarios.ToListAsync();
                var listUsers = new List<Usuario>();
                var usersfiltradas = new List<UsuarioDto>();
                List<int> IdUserList = UsersList(id_exam);
                foreach (var id in IdUserList)
                {
                    var user = users.FirstOrDefault(c => c.Id_Usuario == id);
                    if (user != null)
                    {
                        listUsers.Add(user);
                    }
                }
                if (listUsers.Count == 0)
                {
                    return NotFound("No hay usuarios disponible");
                }

                foreach (var user in listUsers)
                {
                    var userdto = new UsuarioDto
                    {
                        Id = user.Id_Usuario,
                        Nombre = user.Nombre,
                        Rol = user.Rol,
                        Correo = user.Correo,
                        Imagen = user.Imagen,
                    };

                    usersfiltradas.Add(userdto);
                }

                return Ok(usersfiltradas);
            }
            return NotFound("No existe la clase");
        }
        private bool ExamenExiste(int id)
        {
            return _context.Examenes.Any(e => e.Id_Clase == id);
        }


        private List<int> UsersList(int id)
        {
            var IdUsers = _context.Estudi_Examenes.Where(e => e.Id_Examen == id).ToList();
            var IdUsersList = new List<int>();
            foreach (var item in IdUsers)
            {
                if (item.Id_Estudiane != null)
                {
                    IdUsersList.Add((int)item.Id_Estudiane);
                }
            }
            var Iduserslis = IdUsersList.Distinct().ToList();
            return Iduserslis;
        }
    }
}
