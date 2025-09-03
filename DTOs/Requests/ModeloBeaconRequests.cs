using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação (API → Entity)</summary>
    public class CreateModeloBeaconDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = default!;

        [StringLength(100, ErrorMessage = "O fabricante não pode ter mais de 100 caracteres")]
        public string? FABRICANTE { get; set; }
    }

    /// <summary>DTO de atualização parcial (API → Entity)</summary>
    public class UpdateModeloBeaconDTO
    {
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string? NOME { get; set; }

        [StringLength(100, ErrorMessage = "O fabricante não pode ter mais de 100 caracteres")]
        public string? FABRICANTE { get; set; }
    }
}