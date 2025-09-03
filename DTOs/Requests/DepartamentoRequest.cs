using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    /// <summary>DTO de entrada para criação de Departamento (API → Entity)</summary>
    public class CreateDepartamentoDTO
    {
        [Required, StringLength(120)]
        public string NOME { get; set; } = default!;

        [Required]
        public int ID_FILIAL { get; set; }
    }

    /// <summary>DTO de entrada para atualização de Departamento (API → Entity)</summary>
    /// <remarks>Campos opcionais para suportar atualização parcial; nulos são ignorados no mapeamento.</remarks>
    public class UpdateDepartamentoDTO
    {
        [StringLength(120)]
        public string? NOME { get; set; }

        public int? ID_FILIAL { get; set; }
    }
}