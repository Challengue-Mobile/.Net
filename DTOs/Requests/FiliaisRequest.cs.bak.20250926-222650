using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação (API → Entity)</summary>
    public class CreateFilialDto
    {
        [Required, StringLength(120)]
        public string NOME { get; set; } = default!;

        public int? ID_PATIO { get; set; }
    }

    /// <summary>DTO de atualização (API → Entity)</summary>
    /// <remarks>Campos opcionais para suportar update parcial.</remarks>
    public class UpdateFilialDto
    {
        [StringLength(120)]
        public string? NOME { get; set; }

        public int? ID_PATIO { get; set; }
    }
}