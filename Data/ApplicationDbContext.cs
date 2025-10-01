using Microsoft.EntityFrameworkCore;
using MottothTracking.Models;

namespace MottothTracking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Beacon> Beacons => Set<Beacon>();
        public DbSet<Moto> Motos => Set<Moto>();
        public DbSet<Localizacao> Localizacoes => Set<Localizacao>();
        public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();
        public DbSet<Patio> Patios => Set<Patio>();
        public DbSet<Zona> Zonas => Set<Zona>();
        public DbSet<RegistroBateria> RegistrosBateria => Set<RegistroBateria>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<LogSistema> LogsSistema => Set<LogSistema>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índices e relacionamentos principais
            modelBuilder.Entity<Beacon>()
                .HasIndex(x => x.Uuid).IsUnique(false);

            modelBuilder.Entity<Zona>()
                .HasOne(z => z.Patio)
                .WithMany(p => p.Zonas)
                .HasForeignKey(z => z.PatioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Moto)
                .WithMany(m => m.Localizacoes)
                .HasForeignKey(l => l.MotoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Zona)
                .WithMany()
                .HasForeignKey(l => l.ZonaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Moto)
                .WithMany(mo => mo.Movimentacoes)
                .HasForeignKey(m => m.MotoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.ZonaOrigem)
                .WithMany()
                .HasForeignKey(m => m.ZonaOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.ZonaDestino)
                .WithMany()
                .HasForeignKey(m => m.ZonaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LogSistema>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed mínimo exigido pela sprint (2 registros reais)
            modelBuilder.Entity<Beacon>().HasData(
                new Beacon { Id = 1, Uuid = "33333333-3333-3333-3333-333333333333", Modelo = "FIAP-DEV",  Status = "ATIVO",      Bateria = 90 },
                new Beacon { Id = 2, Uuid = "44444444-4444-4444-4444-444444444444", Modelo = "FIAP-TEST", Status = "MANUTENCAO", Bateria = 75 }
            );
        }
    }
}
