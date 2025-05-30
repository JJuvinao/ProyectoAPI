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

        //[Authorize(Roles = "Profesor")]
        //[HttpPut("Calificar/{id_Usuario}")]
        //public async Task<ActionResult> PutCalificar(Estu_ExamPut estu_Exam, int id_Usuario)
        //{
        //    var eventoExistente = await _context.Eventos.FindAsync(id);
        //    if (eventoExistente == null)
        //    {
        //        return NotFound("Evento no existe");
        //    }

        //    eventoExistente.Nombre_Evento = evento.Nombre_Evento;
        //    eventoExistente.Descripcion = evento.Descripcion;
        //    eventoExistente.Nombre_Lugar = evento.Nombre_Lugar;
        //    eventoExistente.Direccion_Lugar = evento.Direccion_Lugar;
        //    eventoExistente.Aforo_Max = evento.Aforo_Max;
        //    eventoExistente.PrecioTicket = evento.PrecioTicket;
        //    eventoExistente.Tickets_Disponible = evento.Tickets_Disponible;
        //    eventoExistente.Estado = evento.Estado;
        //    eventoExistente.Categoria = evento.Categoria;


        //    _context.Entry(eventoExistente).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventoExists(id))
        //        {
        //            return NotFound("Fallo la modificacion en base de datos");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok("Modificado");
        //}
    }
}
