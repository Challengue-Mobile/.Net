using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class BeaconDTO
    {
        public int ID_BEACON { get; set; }
        public string UUID { get; set; } = string.Empty;
        public int? BATERIA { get; set; }       // pode vir nulo em leituras antigas
        public int ID_MOTO { get; set; }
        public int ID_MODELO_BEACON { get; set; }
        // Mantemos o contrato enxuto; se quiser exibir nomes (placa/modelo), crie um ViewModel separado.
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateBeaconDto
    {
        [Required(ErrorMessage = "O UUID é obrigatório")]
        [StringLength(100, ErrorMessage = "O UUID não pode ter mais de 100 caracteres")]
        public string UUID { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "O nível de bateria deve estar entre 0 e 100")]
        public int? BATERIA { get; set; }   // opcional na criação (se não informado, o backend pode calcular)

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        [Required(ErrorMessage = "O modelo do beacon é obrigatório")]
        public int ID_MODELO_BEACON { get; set; }
    }

    /// <summary>DTO de atualização parcial (API -> Entity)</summary>
    public class UpdateBeaconDto
    {
        // Todos opcionais para evitar under-posting; o AutoMapper só aplicará se tiver valor (Profile já tem Condition != null)
        [StringLength(100, ErrorMessage = "O UUID não pode ter mais de 100 caracteres")]
        public string? UUID { get; set; }

        [Range(0, 100, ErrorMessage = "O nível de bateria deve estar entre 0 e 100")]
        public int? BATERIA { get; set; }

        public int? ID_MOTO { get; set; }

        public int? ID_MODELO_BEACON { get; set; }
    }
}