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
        [HttpGet ("{IDUser}")]
        public async Task<ActionResult<Clases>> GetId_Clases(int IDUser)
        {
            if (EstudiExists(IDUser))
            {
                var clases = await _context.Clases.ToListAsync();
                var clasesFiltradas = new List<Clases>();
                List<int> IdClasesList = ClasesList(IDUser);
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
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Estudi_Clases>>> PostEstudi_Clase(Estu_ClasDto estu_Clasdto)
        {
            var estudi_Clase = new Estudi_Clases
            {
                Id_Usuario = estu_Clasdto.Id_Usuario,
                Id_Clase = estu_Clasdto.Id_Clase
            };


            _context.Estudi_Clases.Add(estudi_Clase);
            await _context.SaveChangesAsync();
            return Ok("Guardado");
        }


        private bool EstudiExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id_Usuario == id);
        }
    }
}
