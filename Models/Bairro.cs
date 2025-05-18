namespace API_.Net.Models
{
    public class Bairro
    {
        public int ID_BAIRRO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public int ID_CIDADE { get; set; }
        
        // Propriedades de navegação
        public Cidade? Cidade { get; set; }
        public ICollection<Logradouro>? Logradouros { get; set; }
    }
}