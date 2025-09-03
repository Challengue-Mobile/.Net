using System;

namespace API_.Net.DTOs
{
    // DTO para exibir informações de cliente (Entity -> API)
    public class ClienteDTO
    {
        public int ID_CLIENTE { get; set; }
        public string NOME { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateTime DATA_CADASTRO { get; set; }
        public string EMAIL { get; set; } = string.Empty;
        public string TELEFONE { get; set; } = string.Empty;
        public int QuantidadeMotos { get; set; }
    }
}