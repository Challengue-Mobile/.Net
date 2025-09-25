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

        [Required]
        [Column("NOME")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Column("EMAIL")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("SENHA")]
        [StringLength(255)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [Column("TIPO")]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [Column("DATA_CADASTRO")]
        public DateTime DataCadastro { get; set; }

        // Relacionamentos
        public virtual ICollection<LogSistema> Logs { get; set; } = new List<LogSistema>();
    }
}
