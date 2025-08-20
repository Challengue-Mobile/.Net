using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de cliente
    public class ClienteDto // ← CORRIGIDO - Issue L6
    {
        public int ID_CLIENTE { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateTime DATA_CADASTRO { get; set; }
        public string EMAIL { get; set; } = string.Empty;
        public string TELEFONE { get; set; } = string.Empty;
        public int QuantidadeMotos { get; set; }
    }

    // DTO para criar um novo cliente
    public class CreateClienteDto // ← CORRIGIDO - Issue L18
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

    // DTO para atualizar um cliente existente
    public class UpdateClienteDto // ← CORRIGIDO - Issue L38
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string EMAIL { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "O telefone não pode ter mais de 20 caracteres")]
        public string TELEFONE { get; set; } = string.Empty;
    }
}