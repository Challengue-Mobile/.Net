using System.ComponentModel.DataAnnotations;
using API_.Net.DTOs.Common;

namespace API_.Net.DTOs
{
    /// <summary>DTO de resposta (Entity → API) para funcionário</summary>
    public class FuncionarioDto
    {
        public int ID_FUNCIONARIO { get; set; }

        public int ID_USUARIO { get; set; }
        public int ID_DEPARTAMENTO { get; set; }

        // Campos "decorativos" opcionais para a resposta (preencha via AutoMapper se quiser)
        public string? NOME_USUARIO { get; set; }
        public string? EMAIL_USUARIO { get; set; }
        public string? NOME_DEPARTAMENTO { get; set; }

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public List<Link> Links { get; set; } = new();
    }
}