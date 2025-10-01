using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_PATIO")]
    public class Patio
    {
        [Key]
        [Column("ID_PATIO")]
        public int Id { get; set; }

        [Required, Column("NOME"), StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Column("ENDERECO"), StringLength(200)]
        public string? Endereco { get; set; }

        public virtual ICollection<Zona> Zonas { get; set; } = new List<Zona>();
    }
}
