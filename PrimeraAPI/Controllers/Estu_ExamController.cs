using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

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

        // GET: Estudi_Examen/IDUser
        [HttpGet("{IDUser}")]
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
        [Authorize(Roles = "Estudiante")]
        [HttpPost("IngresarExa/{id_Usuario}")]
        public async Task<ActionResult> PostEstudi_Clase(string codigo, int id_Usuario)
        {
            if (!EstudiExists(id_Usuario))
                return BadRequest("El estudiante no existe");

            var examen = await _context.Examenes.FirstOrDefaultAsync(e => e.Codigo == codigo);
            if (examen == null)
                return NotFound("Codigo de clase ínvalido o inexistente");

            var estudi_exam = new Estudi_Examen
            {
                Id_Estudiane = id_Usuario,
                Id_Examen = examen.Id_Examen,
                Puntaje = null,
                Aciertos = null,
                Fallos = null,
                Tiempo = null,
                Nota = null,
                Recomendacion = null
            };

            _context.Estudi_Examenes.Add(estudi_exam);
            await _context.SaveChangesAsync();

            return Ok("Ingreso a la clase exitoso");
        }

        [Authorize(Roles = "Profesor")]
        [HttpPut("Calificar/{id_Usuario}")]
        public async Task<ActionResult> PutCalificar(Estu_ExamPut estu_Exam, int id_Usuario)
        {
            var RelacionEstudiExam = await _context.Estudi_Examenes
                .FirstOrDefaultAsync(e => e.Id_Estudiane == id_Usuario && e.Id_Examen == estu_Exam.Id_Examen);
            if (RelacionEstudiExam == null)
            {
                return NotFound();
            }

            RelacionEstudiExam.Puntaje = estu_Exam.Puntaje;
            RelacionEstudiExam.Aciertos = estu_Exam.Aciertos;
            RelacionEstudiExam.Fallos = estu_Exam.Fallos;
            RelacionEstudiExam.Tiempo = estu_Exam.Tiempo;
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
    }
}
