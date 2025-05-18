namespace API_.Net.Models
{
    public class Moto
    {
        public int ID_MOTO { get; set; }
        public string PLACA { get; set; } = string.Empty;
        public DateTime DATA_REGISTRO { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int ID_MODELO_MOTO { get; set; }
        
        // Propriedades de navegação
        public Cliente? Cliente { get; set; }
        public ModeloMoto? ModeloMoto { get; set; }
        public ICollection<Localizacao>? Localizacoes { get; set; }
        public ICollection<Movimentacao>? Movimentacoes { get; set; }
        public ICollection<Beacon>? Beacons { get; set; }
    }
}