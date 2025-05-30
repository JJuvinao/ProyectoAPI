using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

namespace PrimeraAPI.Controllers
{
    [Route("api/Estudi_Clases")]
    [ApiController]
    public class Estudi_ClaseController : Controller
    {
        private readonly ContextDB _context;

        public Estudi_ClaseController(ContextDB context)
        {
            _context = context;
        }

        // GET: Estudi_Clase/IDUser
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpGet ("{id_User}")]
        public async Task<ActionResult<Clases>> GetUserClases(int id_User)
        {
            if (EstudiExists(id_User))
            {
                var clases = await _context.Clases.ToListAsync();
                var clasesFiltradas = new List<Clases>();
                List<int> IdClasesList = ClasesList(id_User);
                foreach (var id in IdClasesList)
                {
                    var clase = clases.FirstOrDefault(c => c.Id_Clase == id && c.Estado == true);
                    if (clase != null)
                    {
                        clasesFiltradas.Add(clase);
                    }
                }
                if (clasesFiltradas.Count == 0)
                {
                    return NotFound("No hay clase disponible");
                }
                return Ok(clasesFiltradas);


            }
           return NotFound("No existe el usuario");
        }

        private List<int> ClasesList(int id)
        {
            var IdClases = _context.Estudi_Clases.Where(e => e.Id_Usuario == id).ToList();
            var IdClasesList = new List<int>();
            foreach (var item in IdClases)
            {
                if (item.Id_Clase != null)
                {
                    IdClasesList.Add((int)item.Id_Clase);
                }
            }
            return IdClasesList;
        }

        // POST: Estudi_Clase
        [Authorize(Roles = "Estudiante")]
        [HttpPost("Ingresar/{id_Usuario}")]
        public async Task<ActionResult> PostEstudi_Clase(string codigo, int id_Usuario)
        {
            if (!EstudiExists(id_Usuario))
                return BadRequest("El estudiante no existe");

            var clase = await _context.Clases.FirstOrDefaultAsync(clase => clase.Codigo == codigo);
            if (clase == null)
                return NotFound("Codigo de clase ínvalido o inexistente");

            if (EstudianteEnClase(id_Usuario, clase.Id_Clase))
                return BadRequest("El estudiante ya se encuentra en la clase");

            var estudi_clase = new Estudi_Clases{
                Id_Usuario = id_Usuario,
                Id_Clase = clase.Id_Clase
            };

            _context.Estudi_Clases.Add(estudi_clase);
            await _context.SaveChangesAsync();

            return Ok("Ingreso a la clase exitoso");
        }

        private bool EstudianteEnClase(int id_Usuario, int id_Clase)
        {
            return _context.Estudi_Clases.Any(e => e.Id_Usuario == id_Usuario && e.Id_Clase == id_Clase);
        }

        private bool EstudiExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id_Usuario == id);
        }
    }
}
