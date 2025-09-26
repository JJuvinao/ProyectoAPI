using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Helpers;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrimeraAPI.Controllers
{
    [Route("api/juego")]
    [ApiController]
    public class JuegoController : Controller
    {
        private readonly ContextDB _context;
        private readonly JwtHelper _jwtHelper;

        public JuegoController(ContextDB context, JwtHelper jwtHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Juegos>>> GetJuegos()
        {
            return Ok(await _context.Juegos.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJuegos(int id)
        {
            var juego = await _context.Juegos.FindAsync(id);
            if (juego == null)
            {
                return NotFound("Juego no existe");
            }

            _context.Juegos.Remove(juego);
            await _context.SaveChangesAsync();

            return Ok("Juego eliminado");
        }

    }
}
