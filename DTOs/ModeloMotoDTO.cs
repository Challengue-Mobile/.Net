using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de modelo de moto
    public class ModeloMotoDto // ← CORRIGIDO - Issue L6
    {
        public int ID_MODELO_MOTO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string FABRICANTE { get; set; } = string.Empty;
        public int QuantidadeMotos { get; set; }
    }

    // DTO para criar um novo modelo de moto
    public class CreateModeloMotoDto // ← CORRIGIDO - Issue L15
    {
        [Required(ErrorMessage = "O nome do modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [Required(ErrorMessage = "O fabricante é obrigatório")]
        [StringLength(100, ErrorMessage = "O fabricante não pode ter mais de 100 caracteres")]
        public string FABRICANTE { get; set; } = string.Empty;
    }

    // DTO para atualizar um modelo de moto existente
    public class UpdateModeloMotoDto // ← CORRIGIDO - Issue L27
    {
        [Required(ErrorMessage = "O nome do modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [Required(ErrorMessage = "O fabricante é obrigatório")]
        [StringLength(100, ErrorMessage = "O fabricante não pode ter mais de 100 caracteres")]
        public string FABRICANTE { get; set; } = string.Empty;
    }
}