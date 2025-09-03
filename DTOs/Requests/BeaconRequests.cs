using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateBeaconDto
    {
        [Required] public string UUID { get; set; } = default!;
        [Range(0, 100)] public int? BATERIA { get; set; }
        [Required] public int ID_MOTO { get; set; }
        [Required] public int ID_MODELO_BEACON { get; set; }
    }

    public class UpdateBeaconDto
    {
        public string? UUID { get; set; }
        [Range(0, 100)] public int? BATERIA { get; set; }
        public int? ID_MOTO { get; set; }
        public int? ID_MODELO_BEACON { get; set; }
    }
}