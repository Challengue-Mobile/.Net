using System.ComponentModel.DataAnnotations; // ← ADICIONADO

namespace API_.Net.Models
{
    public class Cliente
    {
        [Required] // ← CORRIGIDO - Issue L5
        public int ID_CLIENTE { get; set; }
        
        public string NOME { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        
        [Required] // ← CORRIGIDO - Issue L8
        public DateTime DATA_CADASTRO { get; set; }
        
        public string EMAIL { get; set; } = string.Empty;
        public string TELEFONE { get; set; } = string.Empty;
        
        // Propriedades de navegação
        public ICollection<Moto>? Motos { get; set; }
    }
}