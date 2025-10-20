using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeraAPI.Models;
using PrimeraAPI.ObjectDto;

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
        public async Task<ActionResult<IEnumerable<ExamenGet>>> GetExamenes()
        {
            var examen = await _context.Examenes.ToListAsync();
            if (examen == null)
            {
                return NotFound("No se encontraron examenes disponibles.");
            }

            var examenesDto = examen.Select(e => new ExamenGet
            {
                Id_Examen = e.Id_Examen,
                Nombre = e.Nombre,
                Tema = e.Tema,
                Autor = e.Autor,
                Descripcion = e.Descripcion,
                Codigo = e.Codigo,
                FechaCreacion = e.FechaCreacion,
                ImagenExamen = e.ImagenExamen,
                Id_Juego = e.Id_Juego
            }).ToList();
            return examenesDto;
        }

        [Authorize]
        [HttpGet("GetAhorcado/{codigo}")]
        public async Task<ActionResult<IEnumerable<ExamenGet>>> GetExamenes_Ahorcado(string codigo)
        {
            var exa_ahorcado = await _context.Palabras_Ahorcados.FirstOrDefaultAsync(e => e.Codigo_Exa == codigo);

            if (exa_ahorcado == null) 
            {
                return NotFound("No se encontro el contenido del examen");
            }

            var ahorcado = new AhorcadoDto
            {
                Palabra = exa_ahorcado.Palabra,
                Pista = exa_ahorcado.Pista,
            };

            return Ok(ahorcado);
        }


        [HttpGet("GetHeroes/{codigo}")]
        public async Task<ActionResult<IEnumerable<ExamenGet>>> GetExamenes_Heroes(string codigo)
        {
            var exa_heroes = await _context.Preguntas_Heroes.Where(e => e.Codigo_Exa == codigo).ToListAsync();

            if (exa_heroes == null)
            {
                return NotFound("No se encontro el contenido del examen");
            }

            var heroeslist = new List<Preg_HeroesDto>();
            foreach (var exa in exa_heroes) 
            {
                var heroes = new Preg_HeroesDto
                {
                    Pregunta = exa.Pregunta,
                    RespuestaV = exa.RespuestaV,
                    RespuestaF1 = exa.RespuestaF1,
                    RespuestaF2 = exa.RespuestaF2,
                    RespuestaF3 = exa.RespuestaF3
                };
                heroeslist.Add(heroes);
            }

            return Ok(heroeslist);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamenes(int id, Examenes examenes)
        {
            if (id != examenes.Id_Examen)
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
                    return NotFound("no existe el examne");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Examenes/Ahorcado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Ahorcado")]
        public async Task<ActionResult<Examenes>> PostExamenes_Ahorcado(Examen_AhorcadoDto examenes)
        {
            string codigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            var Examenes = new Examenes
            {
                Nombre = examenes.Nombre,
                Tema = examenes.Tema,
                Autor = examenes.Autor,
                Descripcion = examenes.Descripcion,
                Codigo = codigo,
                Estado = true, 
                FechaCreacion = DateTime.Now,
                ImagenExamen = examenes.ImagenExamen,
                Id_Clase = examenes.Id_Clase,
                Id_Juego = examenes.Id_Juego
            };

            _context.Examenes.Add(Examenes);

            if (!string.IsNullOrEmpty(examenes.Palabra) && !string.IsNullOrEmpty(examenes.Pista))
            {
                var ahoracado = new Palabras_Ahorcado
                {
                    Palabra = examenes.Palabra,
                    Pista = examenes.Pista,
                    Codigo_Exa = codigo,
                };
                _context.Palabras_Ahorcados.Add(ahoracado);
            }
            await _context.SaveChangesAsync();
            return Ok("Examen creado con exito");
        }

        // POST: api/Examenes/Heroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Heroes")]
        public async Task<ActionResult<Examenes>> PostExamenes_Heroes(Examen_HeroesDto examenes)
        {
            string codigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            var Examenes = new Examenes
            {
                Nombre = examenes.Nombre,
                Tema = examenes.Tema,
                Autor = examenes.Autor,
                Descripcion = examenes.Descripcion,
                Codigo = codigo,
                Estado = true,
                FechaCreacion = DateTime.Now,
                ImagenExamen = examenes.ImagenExamen,
                Id_Clase = examenes.Id_Clase,
                Id_Juego = examenes.Id_Juego
            };

            _context.Examenes.Add(Examenes);
                if (examenes.Heroes != null)
                {
                    var listheroe = new List<Preguntas_Heroes>();
                    foreach (var her in examenes.Heroes)
                    {
                        var Pr_Heroe = new Preguntas_Heroes
                        {
                            Pregunta = her.Pregunta,
                            RespuestaV = her.RespuestaV,
                            RespuestaF1 = her.RespuestaF1,
                            RespuestaF2 = her.RespuestaF2,
                            RespuestaF3 = her.RespuestaF3,
                            Codigo_Exa = codigo,
                        };
                        listheroe.Add(Pr_Heroe);
                    }
                    if (listheroe.Count > 0)
                    {
                        for (int i = 0; i < listheroe.Count; i++)
                        {
                            _context.Preguntas_Heroes.Add(listheroe[i]);

                        }
                    }
                }
            await _context.SaveChangesAsync();
            return Ok("Examen creado con exito");
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
            return _context.Examenes.Any(e => e.Id_Examen == id);
        }

        [Authorize]
        [HttpGet("ExamenesClase/{id_Clase}")]
        public async Task<ActionResult<IEnumerable<ExamenGet>>> GetExamenesClase(int id_Clase)
        {
            var examenes = await _context.Examenes.Where(e => e.Id_Clase == id_Clase).ToListAsync();
            if (examenes == null)
            {
                return NotFound("No se encontraron examenes disponibles.");
            }

            var examenesDto = examenes.Select(e => new ExamenGet
            {
                Id_Examen = e.Id_Examen,
                Nombre = e.Nombre,
                Tema = e.Tema,
                Autor = e.Autor,
                Descripcion = e.Descripcion,
                Codigo = e.Codigo,
                FechaCreacion = e.FechaCreacion,
                ImagenExamen = e.ImagenExamen,
                Id_Juego = e.Id_Juego
            }).ToList();
            return examenesDto;
        }
    }
}
