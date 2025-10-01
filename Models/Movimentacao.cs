// ... resto dos usings
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_MOVIMENTACAO")]
    public class Movimentacao
    {
        [Key]
        [Column("ID_MOV")]
        public int Id { get; set; }

        [Column("ID_MOTO")]
        public int MotoId { get; set; }
        [ForeignKey(nameof(MotoId))]
        public Moto Moto { get; set; } = null!;

        [Column("ID_ZONA_ORIGEM")]
        public int? ZonaOrigemId { get; set; }
        [ForeignKey(nameof(ZonaOrigemId))]
        public Zona? ZonaOrigem { get; set; }

        [Column("ID_ZONA_DESTINO")]
        public int? ZonaDestinoId { get; set; }
        [ForeignKey(nameof(ZonaDestinoId))]
        public Zona? ZonaDestino { get; set; }

        [Column("DATA_MOVIMENTACAO")]
        public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow;

        [Column("ORIGEM"), StringLength(100)]
        public string? Origem { get; set; }

        [Column("DESTINO"), StringLength(100)]
        public string? Destino { get; set; }

        // 🔹 NOVO: tipo da movimentação (usado no controller)
        [Column("TIPO"), StringLength(30)]
        public string? Tipo { get; set; }
    }
}