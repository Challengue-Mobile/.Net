



















using Xunit;
using FluentAssertions;
using API_.Net.Services;

namespace API.Net.Tests.UnitTests
{
    public class MLServiceTests
    {
        private readonly MLService _mlService;

        public MLServiceTests()
        {
            _mlService = new MLService();
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Prever_Probabilidade_Baixa_Para_Moto_Nova()
        {
            var moto = new MotoData
            {
                Kilometragem = 1000,
                IdadeMoto = 0.5f,
                NumeroRevisoesAtrasadas = 0
            };

            var (probabilidade, necessita, nivel) = _mlService.PreverComDetalhes(moto);

            probabilidade.Should().BeLessThan(40F, "moto nova deve ter baixa necessidade de manutenção");
            necessita.Should().BeFalse("moto nova não deve precisar de manutenção");
            (nivel.Contains("ÓTIMO") || nivel.Contains("BAIXO"))
                .Should().BeTrue("o nível deve indicar boas condições");
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Prever_Probabilidade_Alta_Para_Moto_Antiga()
        {
            var moto = new MotoData
            {
                Kilometragem = 80000,
                IdadeMoto = 7,
                NumeroRevisoesAtrasadas = 5
            };

            var (probabilidade, necessita, nivel) = _mlService.PreverComDetalhes(moto);

            probabilidade.Should().BeGreaterThan(60F, "moto antiga deve ter alta necessidade de manutenção");
            necessita.Should().BeTrue("moto antiga deve precisar de manutenção");
            (nivel.Contains("ALTO") || nivel.Contains("CRÍTICO"))
                .Should().BeTrue("o nível deve refletir manutenção urgente ou alta");
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Prever_Probabilidade_Media_Para_Moto_Usada()
        {
            var moto = new MotoData
            {
                Kilometragem = 25000,
                IdadeMoto = 3,
                NumeroRevisoesAtrasadas = 2
            };

            var (probabilidade, necessita, nivel) = _mlService.PreverComDetalhes(moto);

            probabilidade.Should().BeInRange(40F, 70F, "moto usada deve ter risco médio de manutenção");
            (nivel.Contains("MÉDIO") || nivel.Contains("ALTO") || nivel.Contains("BAIXO"))
                .Should().BeTrue("o nível deve indicar médio, alto ou baixo risco");
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Prever_Manutencao_Falsa_Para_Moto_Nova()
        {
            var moto = new MotoData
            {
                Kilometragem = 5000,
                IdadeMoto = 1,
                NumeroRevisoesAtrasadas = 0
            };

            var (probabilidade, necessita, _) = _mlService.PreverComDetalhes(moto);

            necessita.Should().BeFalse("moto nova e com revisões em dia não precisa de manutenção");
            probabilidade.Should().BeLessThan(50F);
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Prever_Manutencao_Verdadeira_Para_Moto_Desgastada()
        {
            var moto = new MotoData
            {
                Kilometragem = 50000,
                IdadeMoto = 5,
                NumeroRevisoesAtrasadas = 3
            };

            var (probabilidade, necessita, nivel) = _mlService.PreverComDetalhes(moto);

            necessita.Should().BeTrue("moto antiga com revisões atrasadas deve precisar de manutenção");
            probabilidade.Should().BeGreaterThan(60F);
            (nivel.Contains("ALTO") || nivel.Contains("CRÍTICO"))
                .Should().BeTrue("o nível deve indicar alta prioridade");
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Nao_Deve_Quebrar_Com_Dados_Extremos()
        {
            var moto = new MotoData
            {
                Kilometragem = 999999,
                IdadeMoto = 50,
                NumeroRevisoesAtrasadas = 20
            };

            var (probabilidade, necessita, _) = _mlService.PreverComDetalhes(moto);

            probabilidade.Should().BeInRange(0, 100);
            necessita.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Retornar_Valor_Entre_0_e_100()
        {
            var moto = new MotoData
            {
                Kilometragem = 10000,
                IdadeMoto = 2,
                NumeroRevisoesAtrasadas = 1
            };

            var (probabilidade, _, _) = _mlService.PreverComDetalhes(moto);
            probabilidade.Should().BeInRange(0, 100, "toda previsão deve estar entre 0 e 100%");
        }

        [Fact]
        [Trait("Category", "ML")]
        public void Deve_Retornar_Nivel_Condizente_Com_Probabilidade()
        {
            var moto = new MotoData
            {
                Kilometragem = 40000,
                IdadeMoto = 4,
                NumeroRevisoesAtrasadas = 3
            };

            var (probabilidade, _, nivel) = _mlService.PreverComDetalhes(moto);

            if (probabilidade >= 80)
                nivel.Should().Contain("CRÍTICO");
            else if (probabilidade >= 60)
                nivel.Should().Contain("ALTO");
            else if (probabilidade >= 40)
                nivel.Should().Contain("MÉDIO");
            else if (probabilidade >= 20)
                nivel.Should().Contain("BAIXO");
            else
                nivel.Should().Contain("ÓTIMO");
        }
    }
}

