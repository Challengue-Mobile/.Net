using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_LOG_SISTEMA")]
    public class LogSistema
    {
        [Key]
        [Column("ID_LOG")]
        public int Id { get; set; }

        [Required]
        [Column("ACAO")]
        [StringLength(100)]
        public string Acao { get; set; } = string.Empty;

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}
