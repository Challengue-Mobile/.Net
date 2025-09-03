using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateMovimentacaoDto
    {
        public string? OBSERVACAO { get; set; }
        [Required] public int ID_USUARIO { get; set; }
        [Required] public int ID_MOTO { get; set; }
        [Required] public int ID_TIPO_MOVIMENTACAO { get; set; }
    }

    public class UpdateMovimentacaoDto
    {
        public string? OBSERVACAO { get; set; }
        public int? ID_USUARIO { get; set; }
        public int? ID_MOTO { get; set; }
        public int? ID_TIPO_MOVIMENTACAO { get; set; }
    }
}