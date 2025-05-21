using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar um modelo de moto
    /// </summary>
    public class ModeloMotoRequestExample : IExamplesProvider<ModeloMoto>
    {
        public ModeloMoto GetExamples()
        {
            return new ModeloMoto
            {
                NOME = "CB 500",
                FABRICANTE = "Honda"
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter um modelo de moto
    /// </summary>
    public class ModeloMotoResponseExample : IExamplesProvider<ModeloMoto>
    {
        public ModeloMoto GetExamples()
        {
            return new ModeloMoto
            {
                ID_MODELO_MOTO = 1,
                NOME = "CB 500",
                FABRICANTE = "Honda",
                Motos = new List<Moto>
                {
                    new Moto
                    {
                        ID_MOTO = 1,
                        PLACA = "ABC1234"
                    },
                    new Moto
                    {
                        ID_MOTO = 2,
                        PLACA = "DEF5678"
                    }
                }
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de modelos de motos
    /// </summary>
    public class ModelosMotosListResponseExample : IExamplesProvider<ModeloMoto[]>
    {
        public ModeloMoto[] GetExamples()
        {
            return new ModeloMoto[]
            {
                new ModeloMoto
                {
                    ID_MODELO_MOTO = 1,
                    NOME = "CB 500",
                    FABRICANTE = "Honda"
                },
                new ModeloMoto
                {
                    ID_MODELO_MOTO = 2,
                    NOME = "YBR 125",
                    FABRICANTE = "Yamaha"
                },
                new ModeloMoto
                {
                    ID_MODELO_MOTO = 3,
                    NOME = "XRE 300",
                    FABRICANTE = "Honda"
                }
            };
        }
    }
}