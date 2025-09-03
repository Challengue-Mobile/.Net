using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class UsuarioDTO
    {
        public int ID_USUARIO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public DateTime DATA_CADASTRO { get; set; }
        public string EMAIL { get; set; } = string.Empty;
        public int ID_TIPO_USUARIO { get; set; }
        public string TipoUsuario { get; set; } = string.Empty;
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateUsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(255, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres", MinimumLength = 6)]
        public string SENHA { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string EMAIL { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
        public int ID_TIPO_USUARIO { get; set; }
    }

    /// <summary>DTO de atualização (API -> Entity)</summary>
    public class UpdateUsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string EMAIL { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
        public int ID_TIPO_USUARIO { get; set; }
    }

    /// <summary>DTO para alteração de senha</summary>
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "A senha atual é obrigatória")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [StringLength(255, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres", MinimumLength = 6)]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação da senha é obrigatória")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem")]
        public string ConfirmacaoSenha { get; set; } = string.Empty;
    }
}
