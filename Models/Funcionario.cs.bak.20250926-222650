namespace API_.Net.Models
{
    public class Funcionario
    {
        public int ID_FUNCIONARIO { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string CARGO { get; set; } = string.Empty;
        public DateTime DATA_ADMISSAO { get; set; }
        public int? ID_USUARIO { get; set; }
        public int ID_DEPARTAMENTO { get; set; }
        
        // Propriedades de navegação
        public Usuario? Usuario { get; set; }
        public Departamento? Departamento { get; set; }
    }
}