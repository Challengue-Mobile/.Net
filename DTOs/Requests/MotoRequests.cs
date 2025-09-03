using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação (API → Entity)</summary>
    public class CreateMotoDto
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        public string PLACA { get; set; } = default!;

        // Cliente é OPCIONAL no seu domínio (int?)
        public int? ID_CLIENTE { get; set; }

        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public int ID_MODELO_MOTO { get; set; }
    }

    /// <summary>DTO de atualização parcial (API → Entity)</summary>
    public class UpdateMotoDto
    {
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        public string? PLACA { get; set; }

        public int? ID_CLIENTE { get; set; }
        public int? ID_MODELO_MOTO { get; set; }
    }
}