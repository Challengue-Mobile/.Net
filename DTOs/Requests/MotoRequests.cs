using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // para JsonRequired

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação (API → Entity)</summary>
    public class CreateMotoDto
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        public string PLACA { get; set; } = default!;

        // Cliente é opcional
        public int? ID_CLIENTE { get; set; }

        [JsonRequired]
        [Range(1, int.MaxValue, ErrorMessage = "O modelo da moto deve ser maior que zero")]
        public int ID_MODELO_MOTO { get; set; }
    }

    /// <summary>DTO de atualização parcial (API → Entity)</summary>
    public class UpdateMotoDto
    {
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        public string? PLACA { get; set; }

        public int? ID_CLIENTE { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O modelo da moto deve ser maior que zero")]
        public int? ID_MODELO_MOTO { get; set; }
    }
}