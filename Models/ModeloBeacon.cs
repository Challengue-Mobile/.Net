namespace API_.Net.Models
{
    public class ModeloBeacon
    {
        public int ID_MODELO_BEACON { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string FABRICANTE { get; set; } = string.Empty;
        
        // Propriedades de navegação
        public ICollection<Beacon>? Beacons { get; set; }
    }
}