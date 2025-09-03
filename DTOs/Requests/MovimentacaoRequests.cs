using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação (API → Entity)</summary>
    public class CreateMovimentacaoDto
    {
        [StringLength(255, ErrorMessage = "A observação não pode ter mais de 255 caracteres")]
        public string? OBSERVACAO { get; set; }

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public int ID_USUARIO { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório")]
        public int ID_TIPO_MOVIMENTACAO { get; set; }
    }

    /// <summary>DTO de atualização parcial (API → Entity)</summary>
    public class UpdateMovimentacaoDto
    {
        [StringLength(255, ErrorMessage = "A observação não pode ter mais de 255 caracteres")]
        public string? OBSERVACAO { get; set; }

        public int? ID_USUARIO { get; set; }
        public int? ID_MOTO { get; set; }
        public int? ID_TIPO_MOVIMENTACAO { get; set; }
    }
}