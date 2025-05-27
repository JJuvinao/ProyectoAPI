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
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Estu_ExamFrom>>> PostEstudi_Examen(Estu_ExamDto estu_ExamDto)
        {
            if (estu_ExamDto == null)
            {
                return BadRequest("Datos inválidos");
            }

            var estu_Examen = await _context.Estudi_Examenes
                .Where(e => e.Id_Estudiane == estu_ExamDto.Id_Estudiane
                && e.Id_Examen == estu_ExamDto.Id_Examen)
                .FirstOrDefaultAsync();

            if (estu_Examen != null)
            {
                return NotFound("Ya existe un registro del estudiantes en este examen");
            }

            var newExamen = new Estudi_Examen
            {
                Id_Estudiane = estu_ExamDto.Id_Estudiane,
                Id_Examen = estu_ExamDto.Id_Examen
            };

            _context.Estudi_Examenes.Add(newExamen);
            await _context.SaveChangesAsync();
            return Ok("Guardado correctamente");

        }
    }
}
