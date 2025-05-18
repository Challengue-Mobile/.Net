namespace API_.Net.Models
{
    public class RegistroBateria
    {
        public int ID_REGISTRO { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int NIVEL_BATERIA { get; set; }
        public int ID_BEACON { get; set; }
        
        // Propriedades de navegação
        public Beacon? Beacon { get; set; }
    }
}