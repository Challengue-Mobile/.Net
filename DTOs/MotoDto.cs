using API_.Net.DTOs.Common;

namespace API_.Net.DTOs
{
    /// <summary>DTO de resposta (Entity → API) para Moto</summary>
    public class MotoDto
    {
        public int ID_MOTO { get; set; }
        public string PLACA { get; set; } = string.Empty;
        public System.DateTime DATA_REGISTRO { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int ID_MODELO_MOTO { get; set; }

        // Campos "derivados"/join
        public string NomeCliente { get; set; } = string.Empty;
        public string ModeloMoto { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public List<Link> Links { get; set; } = new();
    }
}