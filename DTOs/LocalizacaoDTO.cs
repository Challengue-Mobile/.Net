using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class LocalizacaoDTO
    {
        public int ID_LOCALIZACAO { get; set; }
        public decimal POSICAO_X { get; set; }
        public decimal POSICAO_Y { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }
        public string PlacaMoto { get; set; } = string.Empty;
        public string? NomePatio { get; set; }
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateLocalizacaoDto
    {
        [Required(ErrorMessage = "A posição X é obrigatória")]
        [Range(-90, 90, ErrorMessage = "A latitude deve estar entre -90 e 90 graus")]
        public decimal POSICAO_X { get; set; }

        [Required(ErrorMessage = "A posição Y é obrigatória")]
        [Range(-180, 180, ErrorMessage = "A longitude deve estar entre -180 e 180 graus")]
        public decimal POSICAO_Y { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        public int? ID_PATIO { get; set; }
    }

    /// <summary>DTO de atualização parcial (API -> Entity)</summary>
    public class UpdateLocalizacaoDto
    {
        [Range(-90, 90, ErrorMessage = "A latitude deve estar entre -90 e 90 graus")]
        public decimal? POSICAO_X { get; set; }

        [Range(-180, 180, ErrorMessage = "A longitude deve estar entre -180 e 180 graus")]
        public decimal? POSICAO_Y { get; set; }

        public int? ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }
    }
}