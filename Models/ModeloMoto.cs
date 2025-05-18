namespace API_.Net.Models
{
    public class ModeloMoto
    {
        public int ID_MODELO_MOTO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string FABRICANTE { get; set; } = string.Empty;
        
        // Propriedades de navegação
        public ICollection<Moto>? Motos { get; set; }
    }
}