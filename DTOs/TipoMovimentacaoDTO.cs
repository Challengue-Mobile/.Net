using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir tipo de movimentação
    public class TipoMovimentacaoDTO
    {
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        public string DESCRICAO { get; set; } = string.Empty;
    }

    // DTO para criar um novo tipo de movimentação
    public class CreateTipoMovimentacaoDTO
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres")]
        public string DESCRICAO { get; set; } = string.Empty;
    }

    // DTO para atualizar um tipo de movimentação existente
    public class UpdateTipoMovimentacaoDTO
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres")]
        public string DESCRICAO { get; set; } = string.Empty;
    }
}