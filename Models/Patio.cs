namespace API_.Net.Models
{
    public class Patio
    {
        public int ID_PATIO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public int ID_LOGRADOURO { get; set; }
        
        // Propriedades de navegação
        public Logradouro? Logradouro { get; set; }
        public ICollection<Localizacao>? Localizacoes { get; set; }
        public ICollection<Filial>? Filiais { get; set; }
    }
}