namespace API_.Net.Models
{
    public class Estado
    {
        public int ID_ESTADO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public int ID_PAIS { get; set; }
        
        // Propriedades de navegação
        public Pais? Pais { get; set; }
        public ICollection<Cidade>? Cidades { get; set; }
    }
}