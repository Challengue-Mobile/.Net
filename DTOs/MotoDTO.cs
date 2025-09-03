using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class MotoDTO
    {
        public int ID_MOTO { get; set; }
        public string PLACA { get; set; } = string.Empty;
        public DateTime DATA_REGISTRO { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int ID_MODELO_MOTO { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string ModeloMoto { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateMotoDTO
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        [RegularExpression(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$", ErrorMessage = "Formato de placa inválido (use AAA0A00 ou AAA0000)")]
        public string PLACA { get; set; } = string.Empty;

        public int? ID_CLIENTE { get; set; }

        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public int ID_MODELO_MOTO { get; set; }
    }

    /// <summary>DTO de atualização (API -> Entity)</summary>
    public class UpdateMotoDTO
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        [RegularExpression(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$", ErrorMessage = "Formato de placa inválido (use AAA0A00 ou AAA0000)")]
        public string PLACA { get; set; } = string.Empty;

        public int? ID_CLIENTE { get; set; }

        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public int ID_MODELO_MOTO { get; set; }
    }
}