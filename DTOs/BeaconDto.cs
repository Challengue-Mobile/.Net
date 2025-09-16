using API_.Net.DTOs.Common;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de beacon (Entity -> API)
    public class BeaconDto
    {
        public int ID_BEACON { get; set; }
        public string UUID { get; set; } = string.Empty;
        public int BATERIA { get; set; }
        public int ID_MOTO { get; set; }
        public int ID_MODELO_BEACON { get; set; }
        public string PlacaMoto { get; set; } = string.Empty;
        public string ModeloBeacon { get; set; } = string.Empty;

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public List<Link> Links { get; set; } = new();
    }
}