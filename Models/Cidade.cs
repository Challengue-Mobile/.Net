namespace API_.Net.Models
{
    public class Cidade
    {
        public int ID_CIDADE { get; set; }
        public string NOME { get; set; } = string.Empty;
        public int ID_ESTADO { get; set; }
        
        // Propriedades de navegação
        public Estado? Estado { get; set; }
        public ICollection<Bairro>? Bairros { get; set; }
    }
}