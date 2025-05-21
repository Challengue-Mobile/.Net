using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar um departamento
    /// </summary>
    public class DepartamentoRequestExample : IExamplesProvider<Departamento>
    {
        public Departamento GetExamples()
        {
            return new Departamento
            {
                NOME = "Recursos Humanos",
                ID_FILIAL = 1
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter um departamento
    /// </summary>
    public class DepartamentoResponseExample : IExamplesProvider<Departamento>
    {
        public Departamento GetExamples()
        {
            return new Departamento
            {
                ID_DEPARTAMENTO = 1,
                NOME = "Recursos Humanos",
                ID_FILIAL = 1,
                Filial = new Filial
                {
                    ID_FILIAL = 1,
                    NOME = "Filial São Paulo"
                }
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de departamentos
    /// </summary>
    public class DepartamentosListResponseExample : IExamplesProvider<Departamento[]>
    {
        public Departamento[] GetExamples()
        {
            return new Departamento[]
            {
                new Departamento
                {
                    ID_DEPARTAMENTO = 1,
                    NOME = "Recursos Humanos",
                    ID_FILIAL = 1
                },
                new Departamento
                {
                    ID_DEPARTAMENTO = 2,
                    NOME = "Financeiro",
                    ID_FILIAL = 1
                },
                new Departamento
                {
                    ID_DEPARTAMENTO = 3,
                    NOME = "Operações",
                    ID_FILIAL = 2
                }
            };
        }
    }
}