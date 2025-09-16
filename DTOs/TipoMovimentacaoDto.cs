using System.ComponentModel.DataAnnotations;
using API_.Net.DTOs.Common;

namespace API_.Net.DTOs
{
    // DTO de saída (Entity -> API)
    public class TipoMovimentacaoDto
    {
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        public string DESCRICAO { get; set; } = string.Empty;

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public List<Link> Links { get; set; } = new();
    }
}