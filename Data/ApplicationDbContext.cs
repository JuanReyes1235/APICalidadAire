using APICalidadAire.Models;
using Microsoft.EntityFrameworkCore;

namespace APICalidadAire.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas de la base de datos.
        public DbSet<SensorCalidadAire> SensoresCalidadAire { get; set; }

        public DbSet<LecturaAire> LecturasAire { get; set; }

        public DbSet<AlertaAire> AlertasAire { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de SensorCalidadAire.
            modelBuilder.Entity<SensorCalidadAire>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Ubicacion).IsRequired().HasMaxLength(150);

                entity.Property(s => s.TipoGas).IsRequired().HasMaxLength(50);

                entity.Property(s => s.Estado).IsRequired().HasMaxLength(30);

                // Configuración de Usuario.
                modelBuilder.Entity<Usuario>(entity =>
                {
                    entity.HasKey(u => u.Id);

                    entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);

                    entity.Property(u => u.Email).IsRequired().HasMaxLength(150);

                    entity.HasIndex(u => u.Email).IsUnique();

                    entity.Property(u => u.PasswordHash).IsRequired();

                    entity.Property(u => u.FechaRegistro).IsRequired();
                });
            });

            // Configuración de LecturaAire.
            modelBuilder.Entity<LecturaAire>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.PM2_5).HasPrecision(10, 2);

                entity.Property(l => l.PM10).HasPrecision(10, 2);

                entity.Property(l => l.CO2).HasPrecision(10, 2);

                entity.HasOne(l => l.Sensor).WithMany(s => s.Lecturas).HasForeignKey(l => l.SensorId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de AlertaAire.
            modelBuilder.Entity<AlertaAire>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Nivel).IsRequired().HasMaxLength(20);

                entity.Property(a => a.Mensaje).IsRequired().HasMaxLength(500);

                entity.HasOne(a => a.Sensor).WithMany(s => s.Alertas).HasForeignKey(a => a.SensorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
