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
        public async Task<ActionResult<IEnumerable<Examenes>>> GetExamenes()
        {
            return await _context.Examenes.ToListAsync();
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
            if (id != examenes.Id)
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
        public async Task<ActionResult<Examenes>> PostExamenes(Examenes examenes)
        {
            _context.Examenes.Add(examenes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamenes", new { id = examenes.Id }, examenes);
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
            return _context.Examenes.Any(e => e.Id == id);
        }
    }
}
