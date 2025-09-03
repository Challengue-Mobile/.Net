using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateLocalizacaoDto
    {
        [Required] public double POSICAO_X { get; set; }
        [Required] public double POSICAO_Y { get; set; }
        [Required] public int ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }
    }

    public class UpdateLocalizacaoDto
    {
        public double? POSICAO_X { get; set; }
        public double? POSICAO_Y { get; set; }
        public int? ID_MOTO { get; set; }
        public int? ID_PATIO { get; set; }
    }
}