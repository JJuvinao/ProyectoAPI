using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

namespace PrimeraAPI.Controllers
{
    [Route("api/Examenes")]
    [ApiController]
    public class ExamenesController : ControllerBase
    {
        private readonly ContextDB _context;

        public ExamenesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Examenes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenGet>>> GetExamenes()
        {
            var examen = await _context.Examenes.ToListAsync();
            if (examen == null)
            {
                return NotFound("No se encontraron examenes disponibles.");
            }

            var examenesDto = examen.Select(e => new ExamenGet
            {
                Id_Examen = e.Id_Examen,
                Nombre = e.Nombre,
                Tema = e.Tema,
                Autor = e.Autor,
                Descripcion = e.Descripcion,
                Codigo = e.Codigo,
                FechaCreacion = e.FechaCreacion,
                ImagenExamen = e.ImagenExamen != null ? Convert.ToBase64String(e.ImagenExamen) : null,
                Id_Juego = e.Id_Juego
            }).ToList();
            return examenesDto;
        }

        // GET: api/Examenes/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Examenes>> GetExamenes(int id)
        {
            var examenes = await _context.Examenes.FindAsync(id);

            if (examenes == null)
            {
                return NotFound();
            }

            return examenes;
        }

        // PUT: api/Examenes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamenes(int id, Examenes examenes)
        {
            if (id != examenes.Id_Examen)
            {
                return BadRequest();
            }

            _context.Entry(examenes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamenesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Examenes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Examenes>> PostExamenes(ExamenDto examenes)
        {
            string codigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            var Examenes = new Examenes
            {
                Nombre = examenes.Nombre,
                Tema = examenes.Tema,
                Autor = examenes.Autor,
                Descripcion = examenes.Descripcion,
                Codigo = codigo,
                Estado = true, 
                FechaCreacion = DateTime.Now,
                Id_Clase = examenes.Id_Clase,
                Id_Juego = examenes.Id_Juego
            };

            if (examenes.ImagenExamen != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await examenes.ImagenExamen.CopyToAsync(memoryStream);
                    Examenes.ImagenExamen = memoryStream.ToArray();
                }
            }

            _context.Examenes.Add(Examenes);
            await _context.SaveChangesAsync();

            return Ok("Examen creado con exito");
        }

        // DELETE: api/Examenes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamenes(int id)
        {
            var examenes = await _context.Examenes.FindAsync(id);
            if (examenes == null)
            {
                return NotFound();
            }

            _context.Examenes.Remove(examenes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamenesExists(int id)
        {
            return _context.Examenes.Any(e => e.Id_Examen == id);
        }
    }
}
