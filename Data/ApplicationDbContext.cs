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

        // DbSets (ajuste conforme suas entidades reais)
        public DbSet<Beacon> Beacons { get; set; } = null!;
        public DbSet<Moto> Motos { get; set; } = null!;
        public DbSet<Movimentacao> Movimentacoes { get; set; } = null!;
        public DbSet<RegistroBateria> RegistrosBateria { get; set; } = null!;
        public DbSet<Localizacao> Localizacoes { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Patio> Patios { get; set; } = null!;
        public DbSet<Zona> Zonas { get; set; } = null!;
        public DbSet<LogSistema> LogsSistema { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Schema do Oracle (ajuste para o seu usuário/schema)
            modelBuilder.HasDefaultSchema("RM558798");

            // ======= BEACON =======
            modelBuilder.Entity<Beacon>(e =>
            {
                e.ToTable("BEACONS");
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                // Uuid -> CODIGO
                e.Property(x => x.Uuid)
                    .HasColumnName("CODIGO")
                    .HasMaxLength(100);

                // Modelo -> DESCRICAO
                e.Property(x => x.Modelo)
                    .HasColumnName("DESCRICAO")
                    .HasMaxLength(255)
                    .IsRequired();

                // Se NÃO existir coluna STATUS no Oracle, ignore a propriedade:
                e.Ignore(x => x.Status);

                // Caso exista a coluna STATUS, troque a linha acima por:
                // e.Property(x => x.Status).HasColumnName("STATUS").HasMaxLength(50);
            });

            // ======= RELACIONAMENTOS =======

            // Beacon 1:N Moto
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Beacon)
                .WithMany(b => b.Motos)
                .HasForeignKey(m => m.BeaconId)
                .OnDelete(DeleteBehavior.SetNull);

            // Beacon 1:N RegistroBateria
            modelBuilder.Entity<RegistroBateria>()
                .HasOne(r => r.Beacon)
                .WithMany(b => b.RegistrosBateria)
                .HasForeignKey(r => r.BeaconId)
                .OnDelete(DeleteBehavior.Cascade);

            // Moto 1:N Movimentacao
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Moto)
                .WithMany(m => m.Movimentacoes)
                .HasForeignKey(m => m.MotoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Moto 1:N Localizacao
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Moto)
                .WithMany(m => m.Localizacoes)
                .HasForeignKey(l => l.MotoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Zona 1:N Localizacao
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Zona)
                .WithMany(z => z.Localizacoes)
                .HasForeignKey(l => l.ZonaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Zona 1:N Movimentacao (origem)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.ZonaOrigem)
                .WithMany(z => z.MovimentacoesOrigem)
                .HasForeignKey(m => m.ZonaOrigemId)
                .OnDelete(DeleteBehavior.SetNull);

            // Zona 1:N Movimentacao (destino)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.ZonaDestino)
                .WithMany(z => z.MovimentacoesDestino)
                .HasForeignKey(m => m.ZonaDestinoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Patio 1:N Zona
            modelBuilder.Entity<Zona>()
                .HasOne(z => z.Patio)
                .WithMany(p => p.Zonas)
                .HasForeignKey(z => z.PatioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Usuario 1:N LogSistema
            modelBuilder.Entity<LogSistema>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
