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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClasGet>>> GetClases()
        {
            var clases = await _context.Clases.ToListAsync();
            var clasedto = clases.Select(c => new ClasGet
            {
                Id_Clase = c.Id_Clase,
                Nombre = c.Nombre,
                Tema = c.Tema,
                Autor = c.Autor,
                Codigo = c.Codigo,
                Estado = c.Estado,
                FechaCreacion = c.FechaCreacion,
                ImagenClase = c.ImagenClase,
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
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpGet("Profe_Clases/{idprofe}")]
        public async Task<ActionResult<IEnumerable<ClasGet>>> GetProfe_Clases(int idprofe)
        {
            var ClasesProfesor = await _context.Clases.Where(e => e.Id_Profe == idprofe).ToListAsync();

            if (ClasesProfesor == null)
            {
                return NotFound("No hay clase disponible");
            }
            var clasedto = ClasesProfesor.Select(c => new ClasGet
            {
                Id_Clase = c.Id_Clase,
                Nombre = c.Nombre,
                Tema = c.Tema,
                Autor = c.Autor,
                Codigo = c.Codigo,
                Estado = c.Estado,
                FechaCreacion = c.FechaCreacion,
                ImagenClase = c.ImagenClase,
                Id_Profe = c.Id_Profe
            }).ToList();

            return Ok(clasedto);
        }

        // POST: api/Clases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin, Profesor")]
        [HttpPost]
        public async Task<ActionResult<Clases>> PostClases(ClasesDto clasesdto)
        {
            string codigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            if (clasesdto == null)
            {
                return NotFound("Clase vacia");
            }

            var clases = new Clases
            {
                Nombre = clasesdto.Nombre,
                Tema = clasesdto.Tema,
                Autor = clasesdto.Autor,
                Codigo = codigo,
                Estado = true,
                FechaCreacion = DateTime.Now,
                Id_Profe = clasesdto.Id_Profe,
                ImagenClase = clasesdto.ImagenClase
            };

            _context.Clases.Add(clases);
            await _context.SaveChangesAsync();

            var mesage = new Mensajes
            {
                Mensaje = "Clase creada correctamente",
                Tipo = codigo            
            };


            return Ok(mesage);
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
