using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateUsuarioDto
    {
        [Required, StringLength(120)] public string NOME { get; set; } = default!;
        [Required, StringLength(60)]  public string SENHA { get; set; } = default!;
        [EmailAddress] public string? EMAIL { get; set; }
        [Required] public int ID_TIPO_USUARIO { get; set; }
    }

    public class UpdateUsuarioDto
    {
        [StringLength(120)] public string? NOME { get; set; }
        [StringLength(60)]  public string? SENHA { get; set; }
        [EmailAddress]      public string? EMAIL { get; set; }
        public int? ID_TIPO_USUARIO { get; set; }
    }
}