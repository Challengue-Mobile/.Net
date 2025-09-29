using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Estado
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_ESTADO { get; set; }
        
        public string NOME { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L7
        public int ID_PAIS { get; set; }
        
        // Propriedades de navegação
        public Pais? Pais { get; set; }
        public ICollection<Cidade>? Cidades { get; set; }
    }
}