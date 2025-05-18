using Microsoft.EntityFrameworkCore;
using API_.Net.Models;

namespace API_.Net.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets para todas as entidades
        public DbSet<Moto> Motos { get; set; }
        public DbSet<ModeloMoto> ModelosMotos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<TipoMovimentacao> TiposMovimentacao { get; set; }
        public DbSet<Beacon> Beacons { get; set; }
        public DbSet<ModeloBeacon> ModelosBeacon { get; set; }
        public DbSet<RegistroBateria> RegistrosBateria { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Logradouro> Logradouros { get; set; }
        public DbSet<Bairro> Bairros { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das tabelas
            modelBuilder.Entity<Moto>().ToTable("TB_MOTO");
            modelBuilder.Entity<ModeloMoto>().ToTable("TB_MODELO_MOTO");
            modelBuilder.Entity<Cliente>().ToTable("TB_CLIENTE");
            modelBuilder.Entity<Localizacao>().ToTable("TB_LOCALIZACAO");
            modelBuilder.Entity<Movimentacao>().ToTable("TB_MOVIMENTACAO");
            modelBuilder.Entity<TipoMovimentacao>().ToTable("TB_TIPO_MOVIMENTACAO");
            modelBuilder.Entity<Beacon>().ToTable("TB_BEACON");
            modelBuilder.Entity<ModeloBeacon>().ToTable("TB_MODELO_BEACON");
            modelBuilder.Entity<RegistroBateria>().ToTable("TB_REGISTRO_BATERIA");
            modelBuilder.Entity<Usuario>().ToTable("TB_USUARIO");
            modelBuilder.Entity<TipoUsuario>().ToTable("TB_TIPO_USUARIO");
            modelBuilder.Entity<Funcionario>().ToTable("TB_FUNCIONARIO");
            modelBuilder.Entity<Departamento>().ToTable("TB_DEPARTAMENTO");
            modelBuilder.Entity<Filial>().ToTable("TB_FILIAL");
            modelBuilder.Entity<Patio>().ToTable("TB_PATIO");
            modelBuilder.Entity<Logradouro>().ToTable("TB_LOGRADOURO");
            modelBuilder.Entity<Bairro>().ToTable("TB_BAIRRO");
            modelBuilder.Entity<Cidade>().ToTable("TB_CIDADE");
            modelBuilder.Entity<Estado>().ToTable("TB_ESTADO");
            modelBuilder.Entity<Pais>().ToTable("TB_PAIS");
            modelBuilder.Entity<LogSistema>().ToTable("TB_LOG_SISTEMA");

            // Chaves primárias
            modelBuilder.Entity<Moto>().HasKey(m => m.ID_MOTO);
            modelBuilder.Entity<ModeloMoto>().HasKey(mm => mm.ID_MODELO_MOTO);
            modelBuilder.Entity<Cliente>().HasKey(c => c.ID_CLIENTE);
            modelBuilder.Entity<Localizacao>().HasKey(l => l.ID_LOCALIZACAO);
            modelBuilder.Entity<Movimentacao>().HasKey(m => m.ID_MOVIMENTACAO);
            modelBuilder.Entity<TipoMovimentacao>().HasKey(tm => tm.ID_TIPO_MOVIMENTACAO);
            modelBuilder.Entity<Beacon>().HasKey(b => b.ID_BEACON);
            modelBuilder.Entity<ModeloBeacon>().HasKey(mb => mb.ID_MODELO_BEACON);
            modelBuilder.Entity<RegistroBateria>().HasKey(rb => rb.ID_REGISTRO);
            modelBuilder.Entity<Usuario>().HasKey(u => u.ID_USUARIO);
            modelBuilder.Entity<TipoUsuario>().HasKey(tu => tu.ID_TIPO_USUARIO);
            modelBuilder.Entity<Funcionario>().HasKey(f => f.ID_FUNCIONARIO);
            modelBuilder.Entity<Departamento>().HasKey(d => d.ID_DEPARTAMENTO);
            modelBuilder.Entity<Filial>().HasKey(f => f.ID_FILIAL);
            modelBuilder.Entity<Patio>().HasKey(p => p.ID_PATIO);
            modelBuilder.Entity<Logradouro>().HasKey(l => l.ID_LOGRADOURO);
            modelBuilder.Entity<Bairro>().HasKey(b => b.ID_BAIRRO);
            modelBuilder.Entity<Cidade>().HasKey(c => c.ID_CIDADE);
            modelBuilder.Entity<Estado>().HasKey(e => e.ID_ESTADO);
            modelBuilder.Entity<Pais>().HasKey(p => p.ID_PAIS);
            modelBuilder.Entity<LogSistema>().HasKey(l => l.ID_LOG);

            // Relacionamentos

            // Moto -> Cliente (n:1)
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Cliente)
                .WithMany(c => c.Motos)
                .HasForeignKey(m => m.ID_CLIENTE)
                .IsRequired(false);

            // Moto -> ModeloMoto (n:1)
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.ModeloMoto)
                .WithMany(mm => mm.Motos)
                .HasForeignKey(m => m.ID_MODELO_MOTO)
                .IsRequired();

            // Localizacao -> Moto (n:1)
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Moto)
                .WithMany(m => m.Localizacoes)
                .HasForeignKey(l => l.ID_MOTO)
                .IsRequired();

            // Localizacao -> Patio (n:1)
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Patio)
                .WithMany(p => p.Localizacoes)
                .HasForeignKey(l => l.ID_PATIO)
                .IsRequired(false);

            // Movimentacao -> Moto (n:1)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Moto)
                .WithMany(m => m.Movimentacoes)
                .HasForeignKey(m => m.ID_MOTO)
                .IsRequired();

            // Movimentacao -> Usuario (n:1)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Movimentacoes)
                .HasForeignKey(m => m.ID_USUARIO)
                .IsRequired();

            // Movimentacao -> TipoMovimentacao (n:1)
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.TipoMovimentacao)
                .WithMany(tm => tm.Movimentacoes)
                .HasForeignKey(m => m.ID_TIPO_MOVIMENTACAO)
                .IsRequired();

            // Beacon -> Moto (n:1)
            modelBuilder.Entity<Beacon>()
                .HasOne(b => b.Moto)
                .WithMany(m => m.Beacons)
                .HasForeignKey(b => b.ID_MOTO)
                .IsRequired();

            // Beacon -> ModeloBeacon (n:1)
            modelBuilder.Entity<Beacon>()
                .HasOne(b => b.ModeloBeacon)
                .WithMany(mb => mb.Beacons)
                .HasForeignKey(b => b.ID_MODELO_BEACON)
                .IsRequired();

            // RegistroBateria -> Beacon (n:1)
            modelBuilder.Entity<RegistroBateria>()
                .HasOne(rb => rb.Beacon)
                .WithMany(b => b.RegistrosBateria)
                .HasForeignKey(rb => rb.ID_BEACON)
                .IsRequired();

            // Usuario -> TipoUsuario (n:1)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.TipoUsuario)
                .WithMany(tu => tu.Usuarios)
                .HasForeignKey(u => u.ID_TIPO_USUARIO)
                .IsRequired();

            // LogSistema -> Usuario (n:1)
            modelBuilder.Entity<LogSistema>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.ID_USUARIO)
                .IsRequired();

            // Funcionario -> Usuario (n:1)
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Usuario)
                .WithOne()
                .HasForeignKey<Funcionario>(f => f.ID_USUARIO)
                .IsRequired(false);

            // Funcionario -> Departamento (n:1)
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Departamento)
                .WithMany(d => d.Funcionarios)
                .HasForeignKey(f => f.ID_DEPARTAMENTO)
                .IsRequired();

            // Departamento -> Filial (n:1)
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Filial)
                .WithMany(f => f.Departamentos)
                .HasForeignKey(d => d.ID_FILIAL)
                .IsRequired();

            // Filial -> Patio (n:1)
            modelBuilder.Entity<Filial>()
                .HasOne(f => f.Patio)
                .WithMany(p => p.Filiais)
                .HasForeignKey(f => f.ID_PATIO)
                .IsRequired();

            // Patio -> Logradouro (n:1)
            modelBuilder.Entity<Patio>()
                .HasOne(p => p.Logradouro)
                .WithMany(l => l.Patios)
                .HasForeignKey(p => p.ID_LOGRADOURO)
                .IsRequired();

            // Logradouro -> Bairro (n:1)
            modelBuilder.Entity<Logradouro>()
                .HasOne(l => l.Bairro)
                .WithMany(b => b.Logradouros)
                .HasForeignKey(l => l.ID_BAIRRO)
                .IsRequired();

            // Bairro -> Cidade (n:1)
            modelBuilder.Entity<Bairro>()
                .HasOne(b => b.Cidade)
                .WithMany(c => c.Bairros)
                .HasForeignKey(b => b.ID_CIDADE)
                .IsRequired();

            // Cidade -> Estado (n:1)
            modelBuilder.Entity<Cidade>()
                .HasOne(c => c.Estado)
                .WithMany(e => e.Cidades)
                .HasForeignKey(c => c.ID_ESTADO)
                .IsRequired();

            // Estado -> Pais (n:1)
            modelBuilder.Entity<Estado>()
                .HasOne(e => e.Pais)
                .WithMany(p => p.Estados)
                .HasForeignKey(e => e.ID_PAIS)
                .IsRequired();

            // Configurações adicionais para colunas
            modelBuilder.Entity<Moto>().Property(m => m.PLACA).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<Cliente>().Property(c => c.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Cliente>().Property(c => c.CPF).HasMaxLength(14).IsRequired();
            modelBuilder.Entity<Cliente>().Property(c => c.EMAIL).HasMaxLength(100);
            modelBuilder.Entity<Cliente>().Property(c => c.TELEFONE).HasMaxLength(20);
            modelBuilder.Entity<ModeloMoto>().Property(mm => mm.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<ModeloMoto>().Property(mm => mm.FABRICANTE).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Movimentacao>().Property(m => m.OBSERVACAO).HasColumnType("CLOB");
            modelBuilder.Entity<TipoMovimentacao>().Property(tm => tm.DESCRICAO).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Beacon>().Property(b => b.UUID).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<ModeloBeacon>().Property(mb => mb.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<ModeloBeacon>().Property(mb => mb.FABRICANTE).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Usuario>().Property(u => u.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Usuario>().Property(u => u.SENHA).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Usuario>().Property(u => u.EMAIL).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<TipoUsuario>().Property(tu => tu.DESCRICAO).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Funcionario>().Property(f => f.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Funcionario>().Property(f => f.CPF).HasMaxLength(14).IsRequired();
            modelBuilder.Entity<Funcionario>().Property(f => f.CARGO).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Departamento>().Property(d => d.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Filial>().Property(f => f.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Patio>().Property(p => p.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Logradouro>().Property(l => l.NOME).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Bairro>().Property(b => b.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Cidade>().Property(c => c.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Estado>().Property(e => e.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Pais>().Property(p => p.NOME).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<LogSistema>().Property(l => l.ACAO).HasMaxLength(100).IsRequired();
        }
    }
}