using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateMotoDto
    {
        [Required, StringLength(10)] public string PLACA { get; set; } = default!;
        [Required] public int ID_CLIENTE { get; set; }
        [Required] public int ID_MODELO_MOTO { get; set; }
    }

    public class UpdateMotoDto
    {
        [StringLength(10)] public string? PLACA { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int? ID_MODELO_MOTO { get; set; }
    }
}