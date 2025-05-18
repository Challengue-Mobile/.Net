namespace API_.Net.Models
{
    public class Logradouro
    {
        public int ID_LOGRADOURO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public int ID_BAIRRO { get; set; }
        
        // Propriedades de navegação
        public Bairro? Bairro { get; set; }
        public ICollection<Patio>? Patios { get; set; }
    }
}