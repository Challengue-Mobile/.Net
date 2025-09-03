using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class TipoMovimentacaoDTO
    {
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        public string DESCRICAO { get; set; } = string.Empty;
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateTipoMovimentacaoDTO
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres")]
        public string DESCRICAO { get; set; } = string.Empty;
    }

    /// <summary>DTO de atualização (API -> Entity)</summary>
    public class UpdateTipoMovimentacaoDTO
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres")]
        public string DESCRICAO { get; set; } = string.Empty;
    }
}