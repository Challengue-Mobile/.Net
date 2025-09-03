using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    /// <summary>DTO de saída (Entity -> API)</summary>
    public class ModeloMotoDTO
    {
        public int ID_MODELO_MOTO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string FABRICANTE { get; set; } = string.Empty;
        public int QuantidadeMotos { get; set; }
    }

    /// <summary>DTO de criação (API -> Entity)</summary>
    public class CreateModeloMotoDTO
    {
        [Required(ErrorMessage = "O nome do modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [Required(ErrorMessage = "O fabricante é obrigatório")]
        [StringLength(100, ErrorMessage = "O fabricante não pode ter mais de 100 caracteres")]
        public string FABRICANTE { get; set; } = string.Empty;
    }

    /// <summary>DTO de atualização (API -> Entity)</summary>
    public class UpdateModeloMotoDTO
    {
        [Required(ErrorMessage = "O nome do modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NOME { get; set; } = string.Empty;

        [Required(ErrorMessage = "O fabricante é obrigatório")]
        [StringLength(100, ErrorMessage = "O fabricante não pode ter mais de 100 caracteres")]
        public string FABRICANTE { get; set; } = string.Empty;
    }
}