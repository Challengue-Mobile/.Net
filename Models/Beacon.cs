using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottothTracking.Models
{
    [Table("TB_BEACON")]
    public class Beacon
    {
        [Key]
        [Column("ID_BEACON")]
        public int Id { get; set; }

        [Required]
        [Column("UUID")]
        [StringLength(100)]
        public string Uuid { get; set; } = string.Empty;

        [Required]
        [Column("MODELO")]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [Column("STATUS")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Column("BATERIA")]
        public int Bateria { get; set; }

        // Relacionamentos
        public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();
        public virtual ICollection<RegistroBateria> RegistrosBateria { get; set; } = new List<RegistroBateria>();
    }
}
