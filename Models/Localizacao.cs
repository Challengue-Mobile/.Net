using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Localizacao
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_LOCALIZACAO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L6
        public decimal POSICAO_X { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L7
        public decimal POSICAO_Y { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L8
        public DateTime DATA_HORA { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L9
        public int ID_MOTO { get; set; }
        
        public int? ID_PATIO { get; set; } // ← JÁ nullable - OK
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public Patio? Patio { get; set; }
    }
}