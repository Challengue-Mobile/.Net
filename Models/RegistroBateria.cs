using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_REGISTRO_BATERIA")]
    public class RegistroBateria
    {
        [Key]
        [Column("ID_REGISTRO")]
        public int Id { get; set; }

        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required]
        [Column("NIVEL_BATERIA")]
        public int NivelBateria { get; set; }

        [Required]
        [Column("ID_BEACON")]
        public int BeaconId { get; set; }

        [ForeignKey("BeaconId")]
        public virtual Beacon? Beacon { get; set; }
    }
}
