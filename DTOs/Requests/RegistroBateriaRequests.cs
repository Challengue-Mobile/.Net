using System.ComponentModel.DataAnnotations;

namespace API_.Net.DTOs.Requests
{
    public class CreateRegistroBateriaDto
    {
        [Range(0,100)] public int NIVEL_BATERIA { get; set; }
        [Required] public int ID_BEACON { get; set; }
    }

    public class UpdateRegistroBateriaDto
    {
        [Range(0,100)] public int? NIVEL_BATERIA { get; set; }
        public int? ID_BEACON { get; set; }
    }
}