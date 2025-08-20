using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Globalization; // ← ADICIONADO

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar um funcionário
    /// </summary>
    public class FuncionarioRequestExample : IExamplesProvider<Funcionario>
    {
        public Funcionario GetExamples()
        {
            return new Funcionario
            {
                NOME = "João da Silva",
                CPF = "123.456.789-00",
                CARGO = "Analista",
                DATA_ADMISSAO = DateTime.Parse("2025-01-15", CultureInfo.InvariantCulture), // ← CORRIGIDO
                ID_DEPARTAMENTO = 1,
                ID_USUARIO = null
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter um funcionário
    /// </summary>
    public class FuncionarioResponseExample : IExamplesProvider<Funcionario>
    {
        public Funcionario GetExamples()
        {
            return new Funcionario
            {
                ID_FUNCIONARIO = 1,
                NOME = "João da Silva",
                CPF = "123.456.789-00",
                CARGO = "Analista",
                DATA_ADMISSAO = DateTime.Parse("2025-01-15", CultureInfo.InvariantCulture), // ← CORRIGIDO
                ID_DEPARTAMENTO = 1,
                ID_USUARIO = null,
                Departamento = new Departamento
                {
                    ID_DEPARTAMENTO = 1,
                    NOME = "Recursos Humanos"
                },
                Usuario = null
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de funcionários
    /// </summary>
    public class FuncionariosListResponseExample : IExamplesProvider<Funcionario[]>
    {
        public Funcionario[] GetExamples()
        {
            return new Funcionario[]
            {
                new Funcionario
                {
                    ID_FUNCIONARIO = 1,
                    NOME = "João da Silva",
                    CPF = "123.456.789-00",
                    CARGO = "Analista",
                    DATA_ADMISSAO = DateTime.Parse("2025-01-15", CultureInfo.InvariantCulture), // ← CORRIGIDO
                    ID_DEPARTAMENTO = 1
                },
                new Funcionario
                {
                    ID_FUNCIONARIO = 2,
                    NOME = "Maria Souza",
                    CPF = "987.654.321-00",
                    CARGO = "Gerente",
                    DATA_ADMISSAO = DateTime.Parse("2024-06-10", CultureInfo.InvariantCulture), // ← CORRIGIDO
                    ID_DEPARTAMENTO = 1
                },
                new Funcionario
                {
                    ID_FUNCIONARIO = 3,
                    NOME = "Pedro Santos",
                    CPF = "456.789.123-00",
                    CARGO = "Coordenador",
                    DATA_ADMISSAO = DateTime.Parse("2024-09-05", CultureInfo.InvariantCulture), // ← CORRIGIDO
                    ID_DEPARTAMENTO = 2
                }
            };
        }
    }
}