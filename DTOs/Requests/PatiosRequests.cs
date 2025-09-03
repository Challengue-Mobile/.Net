using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreatePatioDto
    {
        [Required] public string NOME { get; set; } = default!;
        [Required] public int ID_LOGRADOURO { get; set; }
    }

    public class UpdatePatioDto
    {
        public string? NOME { get; set; }
        public int? ID_LOGRADOURO { get; set; }
    }
}