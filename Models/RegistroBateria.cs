using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class RegistroBateria
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_REGISTRO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L6
        public DateTime DATA_HORA { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L7
        public int NIVEL_BATERIA { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L8
        public int ID_BEACON { get; set; }
        
        // Propriedades de navegação
        public Beacon? Beacon { get; set; }
    }
}