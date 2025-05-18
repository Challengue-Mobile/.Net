using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de localização
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

    // DTO para criar uma nova localização
    public class CreateLocalizacaoDTO
    {
        [Required(ErrorMessage = "A posição X é obrigatória")]
        public decimal POSICAO_X { get; set; }

        [Required(ErrorMessage = "A posição Y é obrigatória")]
        public decimal POSICAO_Y { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        public int? ID_PATIO { get; set; }
    }
}