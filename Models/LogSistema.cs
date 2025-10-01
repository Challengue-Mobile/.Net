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

        [Column("NIVEL"), StringLength(20)]
        public string? Nivel { get; set; }

        [Column("MENSAGEM"), StringLength(500)]
        public string Mensagem { get; set; } = string.Empty;

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; } = DateTime.UtcNow;

        // Controllers esperam LogSistema.Usuario e UsuarioId
        [Column("ID_USUARIO")]
        public int? UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
    }
}
