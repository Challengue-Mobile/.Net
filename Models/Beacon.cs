using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // ← ADICIONADO


namespace API_.Net.Models
{
    public class Beacon
    {
        [Required] // ← CORRIGIDO - Issue L5
        [JsonRequired]
        public int ID_BEACON { get; set; }
        
        [Required] 
        public string UUID { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L7
        [JsonRequired]
        public int BATERIA { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L8
        [JsonRequired]
        public int ID_MOTO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L9
        [JsonRequired]
        public int ID_MODELO_BEACON { get; set; }
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public ModeloBeacon? ModeloBeacon { get; set; }
        public ICollection<RegistroBateria>? RegistrosBateria { get; set; }
    }
}