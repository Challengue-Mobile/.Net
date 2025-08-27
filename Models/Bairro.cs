using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_.Net.Models
{
    public class Bairro
    {
        [Required]                     // Validação no ModelState
        [JsonRequired]                 // Garante presença no JSON
        public int ID_BAIRRO { get; set; }

        [Required]
        public string NOME { get; set; } = string.Empty;

        [Required]
        [JsonRequired]
        public int ID_CIDADE { get; set; }

        // Propriedades de navegação
        public Cidade? Cidade { get; set; }
        public ICollection<Logradouro>? Logradouros { get; set; }
    }
}