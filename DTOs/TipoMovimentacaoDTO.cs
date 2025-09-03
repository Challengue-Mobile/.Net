using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO de saída (Entity -> API)
    public class TipoMovimentacaoDTO
    {
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        public string DESCRICAO { get; set; } = string.Empty;
    }
}