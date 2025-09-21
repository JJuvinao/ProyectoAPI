using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Helpers;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

namespace PrimeraAPI.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ContextDB _context;
        private readonly JwtHelper _jwtHelper;

        public LoginController(ContextDB context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }


        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> PostLogin(Login loginDto)
        {
            if(loginDto == null)
            {
                return Unauthorized("usuario en null");
            }

            var login = await _context.Usuarios
                .FirstOrDefaultAsync(l => l.Nombre == loginDto.username);
            if (login == null || !BCrypt.Net.BCrypt.Verify(loginDto.password, login.Contrasena))
            {
                return Unauthorized("Credenciales inválidas");
            }

            var token = _jwtHelper.GenerateToken(login);
            var user = new UsuarioDto
            {
                Id = login.Id_Usuario,
                Nombre = login.Nombre,
                Rol = login.Rol,
                Correo = login.Correo,
                Imagen = login.Imagen
            };

            return Ok(new
            {
                token,
                user
            });
        }
    }
}
