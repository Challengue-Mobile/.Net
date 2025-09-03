using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateModeloMotoDto
    {
        [Required] public string NOME { get; set; } = default!;
        public string? FABRICANTE { get; set; }
    }

    public class UpdateModeloMotoDto
    {
        public string? NOME { get; set; }
        public string? FABRICANTE { get; set; }
    }
}