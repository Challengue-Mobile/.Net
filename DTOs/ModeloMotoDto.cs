namespace API_.Net.DTOs
{
    /// <summary>DTO de resposta para Modelo de Moto (Entity → API)</summary>
    public class ModeloMotoDto
    {
        public int ID_MODELO_MOTO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string FABRICANTE { get; set; } = string.Empty;
        public int QuantidadeMotos { get; set; }
    }
}