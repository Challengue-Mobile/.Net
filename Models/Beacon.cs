using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Beacon
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_BEACON { get; set; }
        
        public string UUID { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L7
        public int BATERIA { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L8
        public int ID_MOTO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L9
        public int ID_MODELO_BEACON { get; set; }
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public ModeloBeacon? ModeloBeacon { get; set; }
        public ICollection<RegistroBateria>? RegistrosBateria { get; set; }
    }
}