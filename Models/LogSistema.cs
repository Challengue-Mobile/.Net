namespace API_.Net.Models
{
    public class LogSistema
    {
        public int ID_LOG { get; set; }
        public string ACAO { get; set; } = string.Empty;
        public DateTime DATA_HORA { get; set; }
        public int ID_USUARIO { get; set; }
        
        // Propriedades de navegação
        public Usuario? Usuario { get; set; }
    }
}