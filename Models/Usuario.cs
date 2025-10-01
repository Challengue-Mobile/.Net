using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int Id { get; set; }

        [Required, Column("NOME"), StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, Column("EMAIL"), StringLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required, Column("SENHA_HASH"), StringLength(200)]
        public string SenhaHash { get; set; } = string.Empty;

        // Controllers usam DataCadastro
        [Column("DATA_CADASTRO")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public virtual ICollection<LogSistema> Logs { get; set; } = new List<LogSistema>();
    }
}
