using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateTipoUsuarioDTO
    {
        [Required, StringLength(50)]
        public string DESCRICAO { get; set; } = default!;
    }

    public class UpdateTipoUsuarioDTO
    {
        [StringLength(50)]
        public string? DESCRICAO { get; set; }
    }
}