namespace API_.Net.Models
{
    public class Beacon
    {
        public int ID_BEACON { get; set; }
        public string UUID { get; set; } = string.Empty;
        public int BATERIA { get; set; }
        public int ID_MOTO { get; set; }
        public int ID_MODELO_BEACON { get; set; }
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public ModeloBeacon? ModeloBeacon { get; set; }
        public ICollection<RegistroBateria>? RegistrosBateria { get; set; }
    }
}