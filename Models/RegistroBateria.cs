using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_REGISTRO_BATERIA")]
    public class RegistroBateria
    {
        [Key]
        [Column("ID_REG")]
        public int Id { get; set; }

        [Column("ID_BEACON")]
        public int BeaconId { get; set; }
        [ForeignKey(nameof(BeaconId))]
        public Beacon Beacon { get; set; } = null!;

        // Controllers usam NivelBateria (mapear para coluna NIVEL)
        [Column("NIVEL")]
        public int NivelBateria { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
    }
}
