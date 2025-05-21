using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar uma filial
    /// </summary>
    public class FilialRequestExample : IExamplesProvider<Filial>
    {
        public Filial GetExamples()
        {
            return new Filial
            {
                NOME = "Filial São Paulo",
                ID_PATIO = 1
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter uma filial
    /// </summary>
    public class FilialResponseExample : IExamplesProvider<Filial>
    {
        public Filial GetExamples()
        {
            return new Filial
            {
                ID_FILIAL = 1,
                NOME = "Filial São Paulo",
                ID_PATIO = 1,
                Patio = new Patio
                {
                    ID_PATIO = 1,
                    NOME = "Pátio Central"
                },
                Departamentos = new List<Departamento>
                {
                    new Departamento
                    {
                        ID_DEPARTAMENTO = 1,
                        NOME = "Recursos Humanos"
                    },
                    new Departamento
                    {
                        ID_DEPARTAMENTO = 2,
                        NOME = "Financeiro"
                    }
                }
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de filiais
    /// </summary>
    public class FiliaisListResponseExample : IExamplesProvider<Filial[]>
    {
        public Filial[] GetExamples()
        {
            return new Filial[]
            {
                new Filial
                {
                    ID_FILIAL = 1,
                    NOME = "Filial São Paulo",
                    ID_PATIO = 1
                },
                new Filial
                {
                    ID_FILIAL = 2,
                    NOME = "Filial Rio de Janeiro",
                    ID_PATIO = 2
                },
                new Filial
                {
                    ID_FILIAL = 3,
                    NOME = "Filial Belo Horizonte",
                    ID_PATIO = 3
                }
            };
        }
    }
}