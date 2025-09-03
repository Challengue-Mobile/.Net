using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreatePatioDTO
    {
        [Required] public string NOME { get; set; } = default!;
        [Required] public int ID_LOGRADOURO { get; set; }
    }

    public class UpdatePatioDTO
    {
        public string? NOME { get; set; }
        public int? ID_LOGRADOURO { get; set; }
    }
}