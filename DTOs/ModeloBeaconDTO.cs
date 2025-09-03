namespace API_.Net.DTOs
{
    /// <summary>DTO de resposta para Modelo de Beacon (Entity → API)</summary>
    public class ModeloBeaconDTO
    {
        public int ID_MODELO_BEACON { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string FABRICANTE { get; set; } = string.Empty;
    }
}