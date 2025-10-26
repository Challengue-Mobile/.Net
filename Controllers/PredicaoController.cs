using API_.Net.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Asp.Versioning;

namespace API_.Net.Controllers
{
    /// <summary>
    /// Controller para predições usando Machine Learning (ML.NET)
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class PredicaoController : ControllerBase
    {
        private readonly MLService _mlService;
        private readonly ILogger<PredicaoController> _logger;

        public PredicaoController(MLService mlService, ILogger<PredicaoController> logger)
        {
            _mlService = mlService;
            _logger = logger;
        }

        /// <summary>
        /// Prediz a necessidade de manutenção de uma moto usando ML.NET
        /// </summary>
        [HttpPost("prever-manutencao")]
        [ProducesResponseType(typeof(PredicaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<PredicaoResponse> PreverManutencao([FromBody] PredicaoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Dados inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var motoData = new MotoData
                {
                    Kilometragem = request.Kilometragem,
                    IdadeMoto = request.IdadeMoto,
                    NumeroRevisoesAtrasadas = request.NumeroRevisoesAtrasadas
                };

                var (probabilidade, necessitaManutencao, nivel) = _mlService.PreverComDetalhes(motoData);

                var response = new PredicaoResponse
                {
                    Entrada = new
                    {
                        kilometragem = request.Kilometragem,
                        idadeMoto = request.IdadeMoto,
                        revisoesAtrasadas = request.NumeroRevisoesAtrasadas
                    },
                    Predicao = new
                    {
                        scoreProbabilidade = Math.Round((double)probabilidade, 2),
                        necessitaManutencao,
                        nivelPrioridade = nivel,
                        confianca = ObterConfianca(probabilidade)
                    },
                    Recomendacao = ObterRecomendacao(probabilidade, request),
                    ProximasAcoes = ObterProximasAcoes(probabilidade),
                    Metadata = new
                    {
                        algoritmo = "FastTree Regression (ML.NET)",
                        versaoModelo = "1.0",
                        dataPredicao = DateTime.UtcNow,
                        usuarioAutenticado = User.Identity?.Name ?? "Desconhecido"
                    }
                };

                _logger.LogInformation(
                    "Predição realizada: Km={Km}, Score={Score}%, Necessita={Necessita}, Usuário={User}",
                    request.Kilometragem, Math.Round((double)probabilidade, 2), necessitaManutencao, User.Identity?.Name);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar predição de manutenção");
                return StatusCode(500, new
                {
                    message = "Erro ao processar predição",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Realiza predições em lote para múltiplas motos
        /// </summary>
        [HttpPost("prever-manutencao-lote")]
        [ProducesResponseType(typeof(List<PredicaoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<PredicaoResponse>> PreverManutencaoLote([FromBody] List<PredicaoRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest(new { message = "Lista vazia" });
            }

            if (requests.Count > 100)
            {
                return BadRequest(new { message = "Máximo de 100 motos por requisição" });
            }

            var respostas = new List<PredicaoResponse>();

            foreach (var request in requests)
            {
                var motoData = new MotoData
                {
                    Kilometragem = request.Kilometragem,
                    IdadeMoto = request.IdadeMoto,
                    NumeroRevisoesAtrasadas = request.NumeroRevisoesAtrasadas
                };

                var (probabilidade, necessitaManutencao, nivel) = _mlService.PreverComDetalhes(motoData);

                respostas.Add(new PredicaoResponse
                {
                    Entrada = new
                    {
                        kilometragem = request.Kilometragem,
                        idadeMoto = request.IdadeMoto,
                        revisoesAtrasadas = request.NumeroRevisoesAtrasadas
                    },
                    Predicao = new
                    {
                        scoreProbabilidade = Math.Round((double)probabilidade, 2),
                        necessitaManutencao,
                        nivelPrioridade = nivel,
                        confianca = ObterConfianca(probabilidade)
                    },
                    Recomendacao = ObterRecomendacao(probabilidade, request),
                    ProximasAcoes = ObterProximasAcoes(probabilidade)
                });
            }

            _logger.LogInformation("Predição em lote realizada: {Count} motos analisadas", requests.Count);

            return Ok(respostas);
        }

        private string ObterConfianca(float probabilidade)
        {
            return probabilidade switch
            {
                >= 80 or <= 20 => "Alta",
                >= 60 or <= 40 => "Média",
                _ => "Baixa"
            };
        }

        private string ObterRecomendacao(float probabilidade, PredicaoRequest request)
        {
            return probabilidade switch
            {
                >= 80 => $"⚠️ URGENTE: Agende manutenção imediatamente! Com {request.Kilometragem:N0} km e {request.NumeroRevisoesAtrasadas} revisões atrasadas, há alto risco de falha mecânica.",
                >= 60 => $"⚡ ATENÇÃO: Manutenção recomendada em até 7 dias. A moto está com {request.IdadeMoto:N1} anos de uso.",
                >= 40 => $"📋 AGENDAR: Considere agendar revisão nas próximas 2-4 semanas para manter o bom funcionamento.",
                >= 20 => $"✅ MONITORAR: Moto em bom estado. Continue acompanhando e faça revisões preventivas regulares.",
                _ => $"🌟 EXCELENTE: Moto em ótimas condições! Mantenha a manutenção preventiva em dia."
            };
        }

        private List<string> ObterProximasAcoes(float probabilidade)
        {
            return probabilidade switch
            {
                >= 80 => new List<string>
                {
                    "1. Agendar inspeção mecânica completa HOJE",
                    "2. Verificar óleo, freios e correia imediatamente",
                    "3. Não utilizar para viagens longas até revisão",
                    "4. Contatar oficina autorizada urgentemente"
                },
                >= 60 => new List<string>
                {
                    "1. Agendar revisão completa em até 7 dias",
                    "2. Verificar níveis de fluidos",
                    "3. Inspecionar pneus e freios",
                    "4. Preparar orçamento para possíveis reparos"
                },
                >= 40 => new List<string>
                {
                    "1. Agendar revisão preventiva em 2-4 semanas",
                    "2. Monitorar comportamento do motor",
                    "3. Verificar data da última revisão",
                    "4. Preparar documentação do veículo"
                },
                _ => new List<string>
                {
                    "1. Manter calendário de revisões preventivas",
                    "2. Continuar acompanhamento regular",
                    "3. Registrar quilometragem periodicamente",
                    "4. Manter em boas condições de uso"
                }
            };
        }
    }

    public class PredicaoRequest
    {
        [Required(ErrorMessage = "Kilometragem é obrigatória")]
        [Range(0, 500000, ErrorMessage = "Kilometragem deve estar entre 0 e 500.000")]
        public float Kilometragem { get; set; }

        [Required(ErrorMessage = "Idade da moto é obrigatória")]
        [Range(0, 50, ErrorMessage = "Idade deve estar entre 0 e 50 anos")]
        public float IdadeMoto { get; set; }

        [Required(ErrorMessage = "Número de revisões atrasadas é obrigatório")]
        [Range(0, 20, ErrorMessage = "Número de revisões deve estar entre 0 e 20")]
        public float NumeroRevisoesAtrasadas { get; set; }
    }

    public class PredicaoResponse
    {
        public object Entrada { get; set; } = new { };
        public object Predicao { get; set; } = new { };
        public string Recomendacao { get; set; } = string.Empty;
        public List<string> ProximasAcoes { get; set; } = new();
        public object? Metadata { get; set; }
    }
}