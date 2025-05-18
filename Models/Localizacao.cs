namespace API_.Net.Models
{
    public class Localizacao
    {
        public int ID_LOCALIZACAO { get; set; }
        public decimal POSICAO_X { get; set; }
        public decimal POSICAO_Y { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public Patio? Patio { get; set; }
    }
}