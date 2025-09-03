using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    // DTO para criar um novo beacon (API -> Entity)
    public class CreateBeaconDTO
    {
        [Required(ErrorMessage = "O UUID é obrigatório")]
        [StringLength(100, ErrorMessage = "O UUID não pode ter mais de 100 caracteres")]
        public string UUID { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nível de bateria é obrigatório")]
        [Range(0, 100, ErrorMessage = "O nível de bateria deve estar entre 0 e 100")]
        public int BATERIA { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }

        [Required(ErrorMessage = "O modelo do beacon é obrigatório")]
        public int ID_MODELO_BEACON { get; set; }
    }

    // DTO para atualizar um beacon existente (API -> Entity, campos opcionais)
    public class UpdateBeaconDTO
    {
        public string? UUID { get; set; }

        [Range(0, 100, ErrorMessage = "O nível de bateria deve estar entre 0 e 100")]
        public int? BATERIA { get; set; }

        public int? ID_MOTO { get; set; }
        public int? ID_MODELO_BEACON { get; set; }
    }
}