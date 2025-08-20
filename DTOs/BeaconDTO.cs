using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de beacon
    public class BeaconDto // ← CORRIGIDO - Issue L6
    {
        public int ID_BEACON { get; set; }
        public string UUID { get; set; } = string.Empty;
        public int BATERIA { get; set; }
        public int ID_MOTO { get; set; }
        public int ID_MODELO_BEACON { get; set; }
        public string PlacaMoto { get; set; } = string.Empty;
        public string ModeloBeacon { get; set; } = string.Empty;
    }

    // DTO para criar um novo beacon
    public class CreateBeaconDto // ← CORRIGIDO - Issue L18
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

    // DTO para atualizar um beacon existente
    public class UpdateBeaconDto // ← CORRIGIDO - Issue L36
    {
        [Required(ErrorMessage = "O nível de bateria é obrigatório")]
        [Range(0, 100, ErrorMessage = "O nível de bateria deve estar entre 0 e 100")]
        public int BATERIA { get; set; }

        [Required(ErrorMessage = "O ID da moto é obrigatório")]
        public int ID_MOTO { get; set; }
    }
}