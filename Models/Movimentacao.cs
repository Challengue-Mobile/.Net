using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Movimentacao
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_MOVIMENTACAO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L6
        public DateTime DATA_MOVIMENTACAO { get; set; }
        
        public string OBSERVACAO { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L8
        public int ID_USUARIO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L9
        public int ID_MOTO { get; set; }
        
        [Required] // ← CORRIGIDO - Issue L10
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public Usuario? Usuario { get; set; }
        public TipoMovimentacao? TipoMovimentacao { get; set; }
    }
}