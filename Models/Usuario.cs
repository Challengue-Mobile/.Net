namespace API_.Net.Models
{
    public class Usuario
    {
        public int ID_USUARIO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string SENHA { get; set; } = string.Empty;
        public DateTime DATA_CADASTRO { get; set; }
        public string EMAIL { get; set; } = string.Empty;
        public int ID_TIPO_USUARIO { get; set; }
        
        // Propriedades de navegação
        public TipoUsuario? TipoUsuario { get; set; }
        public ICollection<Movimentacao>? Movimentacoes { get; set; }
        public ICollection<LogSistema>? Logs { get; set; }
    }
}