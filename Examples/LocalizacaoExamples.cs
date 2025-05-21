using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para registrar uma localização
    /// </summary>
    public class LocalizacaoRequestExample : IExamplesProvider<Localizacao>
    {
        public Localizacao GetExamples()
        {
            return new Localizacao
            {
                POSICAO_X = -23.550520m,
                POSICAO_Y = -46.633308m,
                ID_MOTO = 1,
                ID_PATIO = null
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter uma localização
    /// </summary>
    public class LocalizacaoResponseExample : IExamplesProvider<Localizacao>
    {
        public Localizacao GetExamples()
        {
            return new Localizacao
            {
                ID_LOCALIZACAO = 1,
                POSICAO_X = -23.550520m,
                POSICAO_Y = -46.633308m,
                DATA_HORA = DateTime.Now.AddHours(-2),
                ID_MOTO = 1,
                ID_PATIO = null,
                Moto = new Moto
                {
                    ID_MOTO = 1,
                    PLACA = "ABC1234"
                },
                Patio = null
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de localizações
    /// </summary>
    public class LocalizacoesListResponseExample : IExamplesProvider<Localizacao[]>
    {
        public Localizacao[] GetExamples()
        {
            return new Localizacao[]
            {
                new Localizacao
                {
                    ID_LOCALIZACAO = 1,
                    POSICAO_X = -23.550520m,
                    POSICAO_Y = -46.633308m,
                    DATA_HORA = DateTime.Now.AddHours(-2),
                    ID_MOTO = 1,
                    ID_PATIO = null
                },
                new Localizacao
                {
                    ID_LOCALIZACAO = 2,
                    POSICAO_X = -23.551458m,
                    POSICAO_Y = -46.634388m,
                    DATA_HORA = DateTime.Now.AddHours(-1),
                    ID_MOTO = 1,
                    ID_PATIO = null
                },
                new Localizacao
                {
                    ID_LOCALIZACAO = 3,
                    POSICAO_X = -23.552396m,
                    POSICAO_Y = -46.635468m,
                    DATA_HORA = DateTime.Now,
                    ID_MOTO = 1,
                    ID_PATIO = 1
                }
            };
        }
    }
}