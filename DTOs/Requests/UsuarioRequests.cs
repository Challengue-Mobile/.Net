using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateUsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = default!;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(255, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres", MinimumLength = 6)]
        public string SENHA { get; set; } = default!;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string EMAIL { get; set; } = default!;

        [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
        public int ID_TIPO_USUARIO { get; set; }
    }

    // Update parcial: propriedades opcionais (null = não alterar)
    public class UpdateUsuarioDTO
    {
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string? NOME { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode ter mais de 100 caracteres")]
        public string? EMAIL { get; set; }

        public int? ID_TIPO_USUARIO { get; set; }
        // Senha NÃO é atualizada aqui. Use ChangePasswordDTO em um endpoint específico.
    }
}