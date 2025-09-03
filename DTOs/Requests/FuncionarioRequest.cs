using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de criação (API → Entity)</summary>
    public class CreateFuncionarioDto
    {
        [Required]
        public int ID_USUARIO { get; set; }

        [Required]
        public int ID_DEPARTAMENTO { get; set; }
    }

    /// <summary>DTO de atualização (API → Entity). Campos opcionais para update parcial.</summary>
    public class UpdateFuncionarioDto
    {
        public int? ID_USUARIO { get; set; }
        public int? ID_DEPARTAMENTO { get; set; }
    }
}