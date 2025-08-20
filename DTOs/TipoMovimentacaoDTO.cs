using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir tipo de movimentação
    public class TipoMovimentacaoDto // ← CORRIGIDO - Issue L6
    {
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        public string DESCRICAO { get; set; } = string.Empty;
    }

    // DTO para criar um novo tipo de movimentação
    public class CreateTipoMovimentacaoDto // ← CORRIGIDO - Issue L13
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres")]
        public string DESCRICAO { get; set; } = string.Empty;
    }

    // DTO para atualizar um tipo de movimentação existente
    public class UpdateTipoMovimentacaoDto // ← CORRIGIDO - Issue L21
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição não pode ter mais de 50 caracteres")]
        public string DESCRICAO { get; set; } = string.Empty;
    }
}