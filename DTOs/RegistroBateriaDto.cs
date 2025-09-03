using System;

namespace API_.Net.DTOs
{
    public class RegistroBateriaDto
    {
        public int ID_REGISTRO { get; set; }
        public int NIVEL_BATERIA { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int ID_BEACON { get; set; }
    }
}