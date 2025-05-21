using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar uma moto
    /// </summary>
    public class MotoRequestExample : IExamplesProvider<Moto>
    {
        public Moto GetExamples()
        {
            return new Moto
            {
                PLACA = "ABC1234",
                ID_CLIENTE = 1,
                ID_MODELO_MOTO = 2
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter uma moto
    /// </summary>
    public class MotoResponseExample : IExamplesProvider<Moto>
    {
        public Moto GetExamples()
        {
            return new Moto
            {
                ID_MOTO = 1,
                PLACA = "ABC1234",
                DATA_REGISTRO = DateTime.Now.AddDays(-30),
                ID_CLIENTE = 1,
                ID_MODELO_MOTO = 2,
                Cliente = new Cliente
                {
                    ID_CLIENTE = 1,
                    NOME = "João da Silva",
                    CPF = "123.456.789-00"
                },
                ModeloMoto = new ModeloMoto
                {
                    ID_MODELO_MOTO = 2,
                    NOME = "CB 500",
                    FABRICANTE = "Honda"
                }
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de motos
    /// </summary>
    public class MotosListResponseExample : IExamplesProvider<Moto[]>
    {
        public Moto[] GetExamples()
        {
            return new Moto[]
            {
                new Moto
                {
                    ID_MOTO = 1,
                    PLACA = "ABC1234",
                    DATA_REGISTRO = DateTime.Now.AddDays(-30),
                    ID_CLIENTE = 1,
                    ID_MODELO_MOTO = 2
                },
                new Moto
                {
                    ID_MOTO = 2,
                    PLACA = "DEF5678",
                    DATA_REGISTRO = DateTime.Now.AddDays(-15),
                    ID_CLIENTE = 2,
                    ID_MODELO_MOTO = 3
                },
                new Moto
                {
                    ID_MOTO = 3,
                    PLACA = "GHI9012",
                    DATA_REGISTRO = DateTime.Now.AddDays(-7),
                    ID_CLIENTE = 1,
                    ID_MODELO_MOTO = 1
                }
            };
        }
    }
}