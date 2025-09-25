using Microsoft.EntityFrameworkCore;
using MottothTracking.Models;

namespace MottothTracking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Beacon> Beacons { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<RegistroBateria> RegistrosBateria { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de relacionamentos
            
            // Beacon -> Moto (1:N)
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Beacon)
                .WithMany(b => b.Motos)
                .HasForeignKey(m => m.BeaconId)
                .OnDelete(DeleteBehavior.SetNull);

            // Beacon -> RegistroBateria (1:N)
            modelBuilder.Entity<RegistroBateria>()
                .HasOne(r => r.Beacon)
                .WithMany(b => b.RegistrosBateria)
                .HasForeignKey(r => r.BeaconId)
                .OnDelete(DeleteBehavior.Cascade);

            // Moto -> Movimentacao (1:N)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Moto)
                .WithMany(m => m.Movimentacoes)
                .HasForeignKey(m => m.MotoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Moto -> Localizacao (1:N)
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Moto)
                .WithMany(m => m.Localizacoes)
                .HasForeignKey(l => l.MotoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Zona -> Localizacao (1:N)
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Zona)
                .WithMany(z => z.Localizacoes)
                .HasForeignKey(l => l.ZonaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Zona -> Movimentacao (Origem) (1:N)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.ZonaOrigem)
                .WithMany(z => z.MovimentacoesOrigem)
                .HasForeignKey(m => m.ZonaOrigemId)
                .OnDelete(DeleteBehavior.SetNull);

            // Zona -> Movimentacao (Destino) (1:N)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.ZonaDestino)
                .WithMany(z => z.MovimentacoesDestino)
                .HasForeignKey(m => m.ZonaDestinoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Patio -> Zona (1:N)
            modelBuilder.Entity<Zona>()
                .HasOne(z => z.Patio)
                .WithMany(p => p.Zonas)
                .HasForeignKey(z => z.PatioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Usuario -> LogSistema (1:N)
            modelBuilder.Entity<LogSistema>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
