using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;

namespace PrimeraAPI.Controllers
{
    [Route("api/Profe_Clase")]
    [ApiController]
    public class Profe_ClaseController : ControllerBase
    {
        private readonly ContextDB _context;

        public Profe_ClaseController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Profe_Clase
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profe_Clase>>> GetProfe_Clases()
        {
            return await _context.Profe_Clases.ToListAsync();
        }

        // GET: api/Profe_Clase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profe_Clase>> GetProfe_Clase(int id)
        {
            var profe_Clase = await _context.Profe_Clases.FindAsync(id);

            if (profe_Clase == null)
            {
                return NotFound();
            }

            return profe_Clase;
        }

        // PUT: api/Profe_Clase/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfe_Clase(int id, Profe_Clase profe_Clase)
        {
            if (id != profe_Clase.id)
            {
                return BadRequest();
            }

            _context.Entry(profe_Clase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Profe_ClaseExists(id))
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

        // POST: api/Profe_Clase
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profe_Clase>> PostProfe_Clase(Profe_Clase profe_Clase)
        {
            _context.Profe_Clases.Add(profe_Clase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfe_Clase", new { id = profe_Clase.id }, profe_Clase);
        }

        // DELETE: api/Profe_Clase/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfe_Clase(int id)
        {
            var profe_Clase = await _context.Profe_Clases.FindAsync(id);
            if (profe_Clase == null)
            {
                return NotFound();
            }

            _context.Profe_Clases.Remove(profe_Clase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Profe_ClaseExists(int id)
        {
            return _context.Profe_Clases.Any(e => e.id == id);
        }
    }
}
