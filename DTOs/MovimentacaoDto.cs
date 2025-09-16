using System;
using API_.Net.DTOs.Common;

namespace API_.Net.DTOs
{
    /// <summary>DTO de resposta (Entity → API) para Movimentação</summary>
    public class MovimentacaoDto
    {
        public int ID_MOVIMENTACAO { get; set; }
        public DateTime DATA_MOVIMENTACAO { get; set; }
        public string OBSERVACAO { get; set; } = string.Empty;
        public int ID_USUARIO { get; set; }
        public int ID_MOTO { get; set; }
        public int ID_TIPO_MOVIMENTACAO { get; set; }

        // Campos "derivados" (joins)
        public string NomeUsuario { get; set; } = string.Empty;
        public string PlacaMoto { get; set; } = string.Empty;
        public string TipoMovimentacao { get; set; } = string.Empty;

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public List<Link> Links { get; set; } = new();
    }
}