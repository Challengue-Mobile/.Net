using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_MOTO")]
    public class Moto
    {
        [Key]
        [Column("ID_MOTO")]
        public int Id { get; set; }

        [Required, Column("PLACA"), StringLength(10)]
        public string Placa { get; set; } = string.Empty;

        [Required, Column("MODELO"), StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required, Column("STATUS"), StringLength(20)]
        public string Status { get; set; } = "ATIVO";   // usado no MotosController

        [Column("DATA_REGISTRO")]
        public DateTime DataRegistro { get; set; } = DateTime.UtcNow;  // usado no MotosController

        [Column("ID_BEACON")]
        public int? BeaconId { get; set; }

        [ForeignKey(nameof(BeaconId))]
        public Beacon? Beacon { get; set; }

        public virtual ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
        public virtual ICollection<Localizacao> Localizacoes { get; set; } = new List<Localizacao>();
    }
}
