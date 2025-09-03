using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateTipoUsuarioDto
    {
        [Required] public string DESCRICAO { get; set; } = default!;
    }

    public class UpdateTipoUsuarioDto
    {
        public string? DESCRICAO { get; set; }
    }
}