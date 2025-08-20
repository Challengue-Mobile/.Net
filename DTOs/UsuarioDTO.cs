using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de usuário
    public class UsuarioDto // ← CORRIGIDO - Issue L6
    {
        public int ID_USUARIO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public DateTime DATA_CADASTRO { get; set; }
        public string EMAIL { get; set; } = string.Empty;
        public int ID_TIPO_USUARIO { get; set; }
        public string TipoUsuario { get; set; } = string.Empty;
    }

    // DTO para criar um novo usuário
    public class CreateUsuarioDto // ← CORRIGIDO - Issue L17
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

    // DTO para atualizar um usuário existente
    public class UpdateUsuarioDto // ← CORRIGIDO - Issue L37
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string EMAIL { get; set; } = string.Empty;

        public int ID_TIPO_USUARIO { get; set; }
    }

    // DTO para alterar senha
    public class ChangePasswordDto // ← CORRIGIDO - Issue L51
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