using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class ClienteDTO
    {
        public int ID_CLIENTE { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateTime DATA_CADASTRO { get; set; }
        public string EMAIL { get; set; } = string.Empty;
        public string TELEFONE { get; set; } = string.Empty;
        public int QuantidadeMotos { get; set; }
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateClienteDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "O CPF deve ter 14 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string EMAIL { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "O telefone não pode ter mais de 20 caracteres")]
        public string TELEFONE { get; set; } = string.Empty;
    }

    /// <summary>DTO de atualização parcial (API -> Entity)</summary>
    public class UpdateClienteDto
    {
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string? NOME { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string? EMAIL { get; set; }

        [StringLength(20, ErrorMessage = "O telefone não pode ter mais de 20 caracteres")]
        public string? TELEFONE { get; set; }
    }
}
