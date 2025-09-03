using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateTipoMovimentacaoDTO
    {
        [Required, StringLength(50)]
        public string DESCRICAO { get; set; } = default!;
    }

    public class UpdateTipoMovimentacaoDTO
    {
        [StringLength(50)]
        public string? DESCRICAO { get; set; }
    }
}