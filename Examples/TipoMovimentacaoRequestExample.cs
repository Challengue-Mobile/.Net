using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar um tipo de movimentação
    /// </summary>
    public class TipoMovimentacaoRequestExample : IExamplesProvider<TipoMovimentacao>
    {
        public TipoMovimentacao GetExamples()
        {
            return new TipoMovimentacao
            {
                DESCRICAO = "Entrada"
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter um tipo de movimentação
    /// </summary>
    public class TipoMovimentacaoResponseExample : IExamplesProvider<TipoMovimentacao>
    {
        public TipoMovimentacao GetExamples()
        {
            return new TipoMovimentacao
            {
                ID_TIPO_MOVIMENTACAO = 1,
                DESCRICAO = "Entrada",
                Movimentacoes = new List<Movimentacao>
                {
                    new Movimentacao
                    {
                        ID_MOVIMENTACAO = 1,
                        ID_MOTO = 1
                    },
                    new Movimentacao
                    {
                        ID_MOVIMENTACAO = 3,
                        ID_MOTO = 2
                    }
                }
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de tipos de movimentação
    /// </summary>
    public class TiposMovimentacaoListResponseExample : IExamplesProvider<TipoMovimentacao[]>
    {
        public TipoMovimentacao[] GetExamples()
        {
            return new TipoMovimentacao[]
            {
                new TipoMovimentacao
                {
                    ID_TIPO_MOVIMENTACAO = 1,
                    DESCRICAO = "Entrada"
                },
                new TipoMovimentacao
                {
                    ID_TIPO_MOVIMENTACAO = 2,
                    DESCRICAO = "Saída"
                },
                new TipoMovimentacao
                {
                    ID_TIPO_MOVIMENTACAO = 3,
                    DESCRICAO = "Manutenção"
                },
                new TipoMovimentacao
                {
                    ID_TIPO_MOVIMENTACAO = 4,
                    DESCRICAO = "Transferência"
                }
            };
        }
    }
}