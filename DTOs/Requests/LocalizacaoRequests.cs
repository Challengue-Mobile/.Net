using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação de Localização</summary>
    public class CreateLocalizacaoDto
    {
        [Required(ErrorMessage = "A posição X é obrigatória")]
        public decimal POSICAO_X { get; set; }

        [Required(ErrorMessage = "A posição Y é obrigatória")]
        public decimal POSICAO_Y { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        public int? ID_PATIO { get; set; }
    }

    /// <summary>DTO de atualização de Localização (parcial)</summary>
    public class UpdateLocalizacaoDto
    {
        public decimal? POSICAO_X { get; set; }
        public decimal? POSICAO_Y { get; set; }
        public int? ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }
    }
}