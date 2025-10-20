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

        [HttpGet]
        public async Task<ActionResult<Clases>> GetEstudi_Clase()
        {
            var estudi_clases = await _context.Estudi_Clases.ToListAsync();
            return Ok(estudi_clases);
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

        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpGet("usersclase/{id_clase}")]
        public async Task<ActionResult<UsuarioDto>> GetUsersclase(int id_clase)
        {
            if (ClaseExists(id_clase))
            {
                var users = await _context.Usuarios.ToListAsync();
                var listUsers = new List<Usuario>();
                var usersfiltradas = new List<UsuarioDto>();
                List<int> IdUserList = UsersList(id_clase);
                foreach (var id in IdUserList)
                {
                    var user = users.FirstOrDefault(c => c.Id_Usuario == id);
                    if (user != null)
                    {
                        listUsers.Add(user);
                    }
                }
                    foreach (var user in listUsers)
                    {
                        var userdto = new UsuarioDto
                        {
                            Id = user.Id_Usuario,
                            Nombre = user.Nombre,
                            Rol = user.Rol,
                            Correo = user.Correo,
                        };

                        usersfiltradas.Add(userdto);
                    }

                    return Ok(usersfiltradas);

            }
            return NotFound("No existe la clase");
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

        private List<int> UsersList(int id)
        {
            var IdUsers = _context.Estudi_Clases.Where(e => e.Id_Clase == id).ToList();
            var IdUsersList = new List<int>();
            foreach (var item in IdUsers)
            {
                if (item.Id_Usuario != null)
                {
                    IdUsersList.Add((int)item.Id_Usuario);
                }
            }
            return IdUsersList;
        }

        // POST: Estudi_Clase
        [Authorize(Roles = "Admin, Profesor, Estudiante")]
        [HttpPost("Ingresar")]
        public async Task<ActionResult<Estu_ClasDto>> PostEstudi_Clase(Estu_ClasDto estu_ClasDto)
        {
            if (!EstudiExists(estu_ClasDto.Id_Usuario))
                return NotFound("El estudiante no existe");

            var clase = await _context.Clases.FirstOrDefaultAsync(clase => clase.Codigo == estu_ClasDto.Codigo);
            if (clase == null)
                return NotFound("Codigo de clase ínvalido o inexistente");

            if (EstudianteEnClase(estu_ClasDto.Id_Usuario, clase.Id_Clase))
                return NotFound("El estudiante ya se encuentra en la clase");

            var estudi_clase = new Estudi_Clases{
                Id_Usuario = estu_ClasDto.Id_Usuario,
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

        private bool ClaseExists(int id)
        {
            return _context.Clases.Any(e => e.Id_Clase == id);
        }


        // POST: Estudi_Clase
        [Authorize(Roles = "Admin, Profesor")]
        [HttpPost("Profe_Clase")]
        public async Task<ActionResult<Estu_ClasDto>> Postprofe_Clase(Estu_ClasDto estu_ClasDto)
        {
            var clase = await _context.Clases.FirstOrDefaultAsync(clase => clase.Codigo == estu_ClasDto.Codigo);
            if (clase == null)
                return NotFound("Codigo de clase ínvalido o inexistente");

            if (EstudianteEnClase(estu_ClasDto.Id_Usuario, clase.Id_Clase))
                return BadRequest("El estudiante ya se encuentra en la clase");

            var estudi_clase = new Estudi_Clases
            {
                Id_Usuario = estu_ClasDto.Id_Usuario,
                Id_Clase = clase.Id_Clase
            };

            _context.Estudi_Clases.Add(estudi_clase);
            await _context.SaveChangesAsync();

            return Ok("Ingreso a la clase exitoso");
        }

        [Authorize(Roles = "Admin, Profesor")]
        [HttpGet("EstudiantesClase/{id_Clase}")]
        public async Task<ActionResult<IEnumerable<UsuarioShow>>> ClaseUsers(int id_Clase)
        {
            var relacionesClases = await _context.Estudi_Clases.Where(e => e.Id_Clase == id_Clase).ToListAsync();
            if (relacionesClases == null)
                return NotFound("No hay estudiantes en esta clase");

            var idsUsuarios = relacionesClases.Select(e => e.Id_Usuario).Distinct().ToList();

            var estudiantes = await _context.Usuarios.Where(u => idsUsuarios.Contains(u.Id_Usuario))
                .Select(u => new UsuarioShow
                {
                    Nombre = u.Nombre,
                    Rol = u.Rol,
                    Imagen = u.Imagen
                }).ToListAsync();

            return estudiantes;
        }
    }
}
