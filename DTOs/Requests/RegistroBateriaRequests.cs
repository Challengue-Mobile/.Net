using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateRegistroBateriaDTO
    {
        [Range(0, 100)] public int NIVEL_BATERIA { get; set; }
        [Required] public int ID_BEACON { get; set; }
    }

    public class UpdateRegistroBateriaDTO
    {
        [Range(0, 100)] public int? NIVEL_BATERIA { get; set; }
        public int? ID_BEACON { get; set; }
    }
}