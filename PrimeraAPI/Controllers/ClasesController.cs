using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

namespace PrimeraAPI.Controllers
{
    [Route("api/Clases")]
    [ApiController]

    public class ClasesController : ControllerBase
    {
        private readonly ContextDB _context;

        public ClasesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Clases
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClasesDto>>> GetClases()
        {
            var clases = await _context.Clases.ToListAsync();
            var clasedto = clases.Select(c => new ClasesDto
            {
                Nombre = c.Nombre,
                Tema = c.Tema,
                Autor = c.Autor,
                Id_Profe = c.Id_Profe
            }).ToList();

            return Ok(clasedto);
        }

        // GET: api/Clases/ids
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpGet("ids")]
        public async Task<ActionResult<Clases>> GetClases(List<int> ids)
        {
            var clases = await _context.Clases.ToListAsync();

            if (clases == null)
            {
                return NotFound("No hay clase disponible");
            }

                var clasesFiltradas = new List<Clases>();
                foreach (var id in ids)
                {
                    var clase = clases.FirstOrDefault(c => c.Id_Clase == id && c.Estado == true);
                    if (clase != null)
                    {
                        clasesFiltradas.Add(clase);
                    }
                }
                if (clasesFiltradas.Count == 0)
                {
                    return NotFound();
                }
                return Ok(clasesFiltradas);
        }

        // GET: Clase/Profe_Clases/idprofe
        [Authorize(Roles = "Admin, Profesor")]
        [HttpGet("Profe_Clases/{idprofe}")]
        public async Task<ActionResult<IEnumerable<Clases>>> GetProfe_Clases(int idprofe)
        {
            var IdClases = await _context.Clases.Where(e => e.Id_Profe == idprofe).ToListAsync();

            if (IdClases == null)
            {
                return NotFound("No hay clase disponible");
            }

            return IdClases;
        }

        // POST: api/Clases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize (Roles = "Admin, Profesor")]
        [HttpPost]
        public async Task<ActionResult<Clases>> PostClases(ClasesDto clasesdto)
        {
            var clases = new Clases
            {
                Nombre = clasesdto.Nombre,
                Tema = clasesdto.Tema,
                Autor = clasesdto.Autor,
                Id_Profe = clasesdto.Id_Profe,
                FechaCreacion = DateTime.Now
            };

            _context.Clases.Add(clases);
            await _context.SaveChangesAsync();

            return Ok("Clase creada correctamente");
        }

        // DELETE: api/Clases/5
        [Authorize(Roles = "Admin, Profesor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClases(int id)
        {
            var clases = await _context.Clases.FindAsync(id);
            if (clases == null)
            {
                return NotFound();
            }

            _context.Clases.Remove(clases);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
