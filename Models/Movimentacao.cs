namespace API_.Net.Models
{
    public class Movimentacao
    {
        public int ID_MOVIMENTACAO { get; set; }
        public DateTime DATA_MOVIMENTACAO { get; set; }
        public string OBSERVACAO { get; set; } = string.Empty;
        public int ID_USUARIO { get; set; }
        public int ID_MOTO { get; set; }
        public int ID_TIPO_MOVIMENTACAO { get; set; }
        
        // Propriedades de navegação
        public Moto? Moto { get; set; }
        public Usuario? Usuario { get; set; }
        public TipoMovimentacao? TipoMovimentacao { get; set; }
    }
}