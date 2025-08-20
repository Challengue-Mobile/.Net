using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Bairro
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_BAIRRO { get; set; }
        
        public string NOME { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L7  
        public int ID_CIDADE { get; set; }
        
        // Propriedades de navegação
        public Cidade? Cidade { get; set; }
        public ICollection<Logradouro>? Logradouros { get; set; }
    }
}