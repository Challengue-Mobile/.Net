namespace API_.Net.DTOs
{
    public class PatioDTO
    {
        public int ID_PATIO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public int ID_LOGRADOURO { get; set; }
        public string? LOGRADOURO_NOME { get; set; }
    }
}