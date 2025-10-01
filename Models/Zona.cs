using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_ZONA")]
    public class Zona
    {
        [Key]
        [Column("ID_ZONA")]
        public int Id { get; set; }

        [Required, Column("NOME"), StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Column("DESCRICAO"), StringLength(255)]
        public string? Descricao { get; set; }

        // Controllers esperam Zona.Patio e Zona.PatioId
        [Column("ID_PATIO")]
        public int PatioId { get; set; }
        [ForeignKey(nameof(PatioId))]
        public Patio Patio { get; set; } = null!;
    }
}
