using Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data
{
    public class HealthCenterContext : DbContext
    {
        public DbSet<EstablecimientoSanitario> EstablecimientoSanitario { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
        public DbSet<Localidad> Localidad { get; set; }
        public DbSet<Provincia> Provincia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // Configure conexion to BBDD
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=HealthCenter;Trusted_Connection=True;Encrypt=False");
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HealthCenter;Trusted_Connection=True;Encrypt=False");
        }
    }
}