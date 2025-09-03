using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateTipoMovimentacaoDto
    {
        [Required] public string DESCRICAO { get; set; } = default!;
    }

    public class UpdateTipoMovimentacaoDto
    {
        public string? DESCRICAO { get; set; }
    }
}