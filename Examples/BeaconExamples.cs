using API_.Net.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de requisição para criar um beacon
    /// </summary>
    public class BeaconRequestExample : IExamplesProvider<Beacon>
    {
        public Beacon GetExamples()
        {
            return new Beacon
            {
                UUID = "550e8400-e29b-41d4-a716-446655440000",
                BATERIA = 85,
                ID_MOTO = 1,
                ID_MODELO_BEACON = 1
            };
        }
    }

    /// <summary>
    /// Exemplo de resposta ao obter um beacon
    /// </summary>
    public class BeaconResponseExample : IExamplesProvider<Beacon>
    {
        public Beacon GetExamples()
        {
            return new Beacon
            {
                ID_BEACON = 1,
                UUID = "550e8400-e29b-41d4-a716-446655440000",
                BATERIA = 85,
                ID_MOTO = 1,
                ID_MODELO_BEACON = 1,
                Moto = new Moto
                {
                    ID_MOTO = 1,
                    PLACA = "ABC1234"
                },
                ModeloBeacon = new ModeloBeacon
                {
                    ID_MODELO_BEACON = 1,
                    NOME = "TrackBeacon Pro",
                    FABRICANTE = "TrackTech"
                }
            };
        }
    }

    /// <summary>
    /// Exemplo de lista de beacons
    /// </summary>
    public class BeaconsListResponseExample : IExamplesProvider<Beacon[]>
    {
        public Beacon[] GetExamples()
        {
            return new Beacon[]
            {
                new Beacon
                {
                    ID_BEACON = 1,
                    UUID = "550e8400-e29b-41d4-a716-446655440000",
                    BATERIA = 85,
                    ID_MOTO = 1,
                    ID_MODELO_BEACON = 1
                },
                new Beacon
                {
                    ID_BEACON = 2,
                    UUID = "550e8400-e29b-41d4-a716-446655440001",
                    BATERIA = 73,
                    ID_MOTO = 2,
                    ID_MODELO_BEACON = 1
                }
            };
        }
    }
}