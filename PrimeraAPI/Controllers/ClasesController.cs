using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;

namespace PrimeraAPI.Controllers
{
    [Route("api/Clases")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<Clases>>> GetClases()
        {
            return await _context.Clases.ToListAsync();
        }

        // GET: api/Clases/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Clases>> GetClases(int id)
        {
            var clases = await _context.Clases.FindAsync(id);

            if (clases == null)
            {
                return NotFound();
            }

            return clases;
        }

        // PUT: api/Clases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClases(int id, Clases clases)
        {
            if (id != clases.Id)
            {
                return BadRequest();
            }

            _context.Entry(clases).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClasesExists(id))
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

        // POST: api/Clases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Clases>> PostClases(Clases clases)
        {
            _context.Clases.Add(clases);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClases", new { id = clases.Id }, clases);
        }

        // DELETE: api/Clases/5
        [Authorize]
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

        private bool ClasesExists(int id)
        {
            return _context.Clases.Any(e => e.Id == id);
        }
    }
}
