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

        [Column("LATITUDE")]
        public double Latitude { get; set; }

        [Column("LONGITUDE")]
        public double Longitude { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; } = DateTime.UtcNow;

        [Column("ID_MOTO")]
        public int MotoId { get; set; }
        [ForeignKey(nameof(MotoId))]
        public Moto Moto { get; set; } = null!;

        // Controllers esperam Localizacao.Zona e ZonaId
        [Column("ID_ZONA")]
        public int ZonaId { get; set; }
        [ForeignKey(nameof(ZonaId))]
        public Zona Zona { get; set; } = null!;
    }
}
