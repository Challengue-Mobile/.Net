using System;

namespace API_.Net.DTOs
{
    /// <summary>DTO de resposta (Entity → API) para Localização</summary>
    public class LocalizacaoDto
    {
        public int ID_LOCALIZACAO { get; set; }
        public decimal POSICAO_X { get; set; }
        public decimal POSICAO_Y { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }

        // Campos derivados (preenchidos via AutoMapper)
        public string PlacaMoto { get; set; } = string.Empty;
        public string? NomePatio { get; set; }
    }
}