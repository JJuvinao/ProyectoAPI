using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Helpers;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

namespace PrimeraAPI.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ContextDB _context;
        private readonly JwtHelper _jwtHelper;

        public UsuariosController(ContextDB context, JwtHelper jwtHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtHelper = jwtHelper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDelete>>> GetUsuarios()
        {
            var usuario = await _context.Usuarios.ToListAsync();

            var usuariosDto = usuario.Select(u => new UserDelete
            {
                Id = u.Id_Usuario,
                Nombre = u.Nombre,
                Rol = u.Rol
            }).ToList();

            return Ok(usuariosDto);
        }

        // GET: api/Usuarios/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioFrom usuarioDto)
        {

                var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Nombre == usuarioDto.Nombre);
                if (existeUsuario)
                {
                    return Conflict("Ya existe un usuario con ese nombre.");
                }

                usuarioDto.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Contrasena);

                var usuario = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Contrasena = usuarioDto.Contrasena,
                    Rol = usuarioDto.Rol,
                    Correo = usuarioDto.Correo

                };

            if (usuarioDto.Imagen != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await usuarioDto.Imagen.CopyToAsync(memoryStream);
                    usuario.Imagen = memoryStream.ToArray();
                }
            }

            try
                {
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return StatusCode(500, "Error al guardar el usuario en la base de datos.");
                }
                return Ok("Usuario registrado");
            }


        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no existe");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok("Uusario eliminado");
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id_Usuario == id);
        }
    }
}
