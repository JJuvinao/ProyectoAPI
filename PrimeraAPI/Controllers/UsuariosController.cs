using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

namespace PrimeraAPI.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ContextDB _context;

        public UsuariosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
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

        [HttpGet("GetUsername/{nombre}")]
        public async Task<ActionResult> GetUsername(string nombre)
        {
            try
            {
                var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Nombre == nombre);
                return existeUsuario ? Conflict() : Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {

                var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Nombre == usuario.Nombre);
                if (existeUsuario)
                {
                    return Conflict("Ya existe un usuario con ese nombre.");
                }

                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

                try
                {
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return StatusCode(500, "Error al guardar el usuario en la base de datos.");
                }

                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Rol = usuario.Rol,
                    Correo = usuario.Correo
                };

                return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuarioDto);
            }

        [HttpPost ("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] Usuario usuario)
        {
            var user = await _context.Usuarios
         .FirstOrDefaultAsync(u => u.Nombre == usuario.Nombre);

            if (user == null)
            {
                return Unauthorized("Usuario no encontrado");
            }
            if (!BCrypt.Net.BCrypt.Verify(usuario.Contrasena, user.Contrasena))
            {
                return Unauthorized("Contraseña incorrecta");
            }

            return Ok(new UsuarioDto(user.Id,user.Nombre,user.Rol,user.Correo));
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
