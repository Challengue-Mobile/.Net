using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class MovimentacaoDTO
    {
        public int ID_MOVIMENTACAO { get; set; }
        public DateTime DATA_MOVIMENTACAO { get; set; }
        public string OBSERVACAO { get; set; } = string.Empty;
        public int ID_USUARIO { get; set; }
        public int ID_MOTO { get; set; }
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string PlacaMoto { get; set; } = string.Empty;
        public string TipoMovimentacao { get; set; } = string.Empty;
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateMovimentacaoDTO
    {
        [StringLength(255, ErrorMessage = "A observação não pode ter mais de 255 caracteres")]
        public string OBSERVACAO { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public int ID_USUARIO { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório")]
        public int ID_TIPO_MOVIMENTACAO { get; set; }
    }

    /// <summary>DTO de atualização (API -> Entity)</summary>
    public class UpdateMovimentacaoDTO
    {
        [StringLength(255, ErrorMessage = "A observação não pode ter mais de 255 caracteres")]
        public string OBSERVACAO { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório")]
        public int ID_TIPO_MOVIMENTACAO { get; set; }
    }
}