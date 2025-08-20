using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Moto
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_MOTO { get; set; }
        
        public string PLACA { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L7
        public DateTime DATA_REGISTRO { get; set; }
        
        public int? ID_CLIENTE { get; set; } // ← JÁ nullable - OK
        
        [Required] // ← CORRIGIDO - Issue L9
        public int ID_MODELO_MOTO { get; set; }
        
        // Propriedades de navegação
        public Cliente? Cliente { get; set; }
        public ModeloMoto? ModeloMoto { get; set; }
        public ICollection<Localizacao>? Localizacoes { get; set; }
        public ICollection<Movimentacao>? Movimentacoes { get; set; }
        public ICollection<Beacon>? Beacons { get; set; }
    }
}