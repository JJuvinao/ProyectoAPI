using Microsoft.EntityFrameworkCore;

namespace PrimeraAPI.Models
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Clases> Clases { get; set; }
        public DbSet<Examenes> Examenes { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Juegos> Juegos { get; set; }
        public DbSet<Estudi_Clases> Estudi_Clases { get; set; }
        public DbSet<Estudi_Examen> Estudi_Examenes { get; set; }
        public DbSet<Productos> Productos { get; set; }

    }
}
