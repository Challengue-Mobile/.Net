using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_MOVIMENTACAO")]
    public class Movimentacao
    {
        [Key]
        [Column("ID_MOVIMENTACAO")]
        public int Id { get; set; }

        [Required]
        [Column("DATA_MOVIMENTACAO")]
        public DateTime DataMovimentacao { get; set; }

        [Required]
        [Column("TIPO_MOVIMENTACAO")]
        [StringLength(20)]
        public string TipoMovimentacao { get; set; } = string.Empty;

        [Column("OBSERVACAO")]
        public string? Observacao { get; set; }

        [Column("FK_ZONA_ORIGEM")]
        public int? ZonaOrigemId { get; set; }

        [Column("FK_ZONA_DESTINO")]
        public int? ZonaDestinoId { get; set; }

        [Required]
        [Column("ID_MOTO")]
        public int MotoId { get; set; }

        [ForeignKey("MotoId")]
        public virtual Moto? Moto { get; set; }

        [ForeignKey("ZonaOrigemId")]
        public virtual Zona? ZonaOrigem { get; set; }

        [ForeignKey("ZonaDestinoId")]
        public virtual Zona? ZonaDestino { get; set; }
    }
}
