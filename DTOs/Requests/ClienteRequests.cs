using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateClienteDto
    {
        [Required, StringLength(120)] public string NOME { get; set; } = default!;
        [Required, StringLength(14)] public string CPF { get; set; } = default!;
        [EmailAddress] public string? EMAIL { get; set; }
        [StringLength(20)] public string? TELEFONE { get; set; }
    }

    public class UpdateClienteDto
    {
        [StringLength(120)] public string? NOME { get; set; }
        [StringLength(14)]  public string? CPF { get; set; }
        [EmailAddress]      public string? EMAIL { get; set; }
        [StringLength(20)]  public string? TELEFONE { get; set; }
    }
}