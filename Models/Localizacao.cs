using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_LOCALIZACAO")]
    public class Localizacao
    {
        [Key]
        [Column("ID_LOCALIZACAO")]
        public int Id { get; set; }

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Column("POSICAO_X")]
        public decimal PosicaoX { get; set; }

        [Required]
        [Column("POSICAO_Y")]
        public decimal PosicaoY { get; set; }

        [Required]
        [Column("ID_MOTO")]
        public int MotoId { get; set; }

        [Required]
        [Column("ID_ZONA")]
        public int ZonaId { get; set; }

        [ForeignKey("MotoId")]
        public virtual Moto? Moto { get; set; }

        [ForeignKey("ZonaId")]
        public virtual Zona? Zona { get; set; }
    }
}
