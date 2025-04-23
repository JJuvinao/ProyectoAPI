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
        public DbSet<Profe_Clase> Profe_Clases { get; set; }
    }
}
