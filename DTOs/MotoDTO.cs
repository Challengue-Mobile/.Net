using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de moto
    public class MotoDTO
    {
        public int ID_MOTO { get; set; }
        public string PLACA { get; set; } = string.Empty;
        public DateTime DATA_REGISTRO { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int ID_MODELO_MOTO { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string ModeloMoto { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
    }

    // DTO para criar uma nova moto
    public class CreateMotoDTO
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        public string PLACA { get; set; } = string.Empty;

        public int? ID_CLIENTE { get; set; }

        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public int ID_MODELO_MOTO { get; set; }
    }

    // DTO para atualizar uma moto existente
    public class UpdateMotoDTO
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(10, ErrorMessage = "A placa não pode ter mais de 10 caracteres")]
        public string PLACA { get; set; } = string.Empty;

        public int? ID_CLIENTE { get; set; }

        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public int ID_MODELO_MOTO { get; set; }
    }
}