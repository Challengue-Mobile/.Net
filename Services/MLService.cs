using Microsoft.ML;
using Microsoft.ML.Data;

namespace API_.Net.Services
{
    public class MotoData
    {
        [LoadColumn(0)]
        public float Kilometragem { get; set; }

        [LoadColumn(1)]
        public float IdadeMoto { get; set; }

        [LoadColumn(2)]
        public float NumeroRevisoesAtrasadas { get; set; }

        [LoadColumn(3)]
        public bool Label { get; set; } // agora é binário (precisa de manutenção ou não)
    }

    public class ManutencaoPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class MLService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;
        private readonly ILogger<MLService>? _logger;

        public MLService(ILogger<MLService>? logger = null)
        {
            _mlContext = new MLContext(seed: 42);
            _logger = logger;
            TreinarModelo();
        }

        private void TreinarModelo()
        {
            try
            {
                _logger?.LogInformation("🚀 Iniciando treinamento do modelo ML.NET...");

                var dadosTreinamento = new[]
                {
                    new MotoData { Kilometragem = 1000, IdadeMoto = 0.5f, NumeroRevisoesAtrasadas = 0, Label = false },
                    new MotoData { Kilometragem = 5000, IdadeMoto = 1, NumeroRevisoesAtrasadas = 0, Label = false },
                    new MotoData { Kilometragem = 15000, IdadeMoto = 2, NumeroRevisoesAtrasadas = 1, Label = false },
                    new MotoData { Kilometragem = 25000, IdadeMoto = 3, NumeroRevisoesAtrasadas = 2, Label = true },
                    new MotoData { Kilometragem = 40000, IdadeMoto = 4, NumeroRevisoesAtrasadas = 3, Label = true },
                    new MotoData { Kilometragem = 50000, IdadeMoto = 5, NumeroRevisoesAtrasadas = 3, Label = true },
                    new MotoData { Kilometragem = 80000, IdadeMoto = 7, NumeroRevisoesAtrasadas = 5, Label = true },
                    new MotoData { Kilometragem = 100000, IdadeMoto = 8, NumeroRevisoesAtrasadas = 6, Label = true },
                };

                var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

                var pipeline = _mlContext.Transforms.Concatenate("Features",
                        nameof(MotoData.Kilometragem),
                        nameof(MotoData.IdadeMoto),
                        nameof(MotoData.NumeroRevisoesAtrasadas))
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    // Classificação binária (0 = não precisa, 1 = precisa)
                    .Append(_mlContext.BinaryClassification.Trainers.FastForest(
                        labelColumnName: nameof(MotoData.Label),
                        featureColumnName: "Features",
                        numberOfLeaves: 20,
                        numberOfTrees: 100,
                        minimumExampleCountPerLeaf: 5));

                _model = pipeline.Fit(dataView);

                _logger?.LogInformation("✅ Modelo ML.NET treinado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "❌ Erro ao treinar modelo ML.NET");
                throw;
            }
        }

        public (float probabilidade, bool necessitaManutencao, string nivel) PreverComDetalhes(MotoData moto)
        {
            if (_model == null)
                throw new InvalidOperationException("Modelo não foi treinado");

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MotoData, ManutencaoPrediction>(_model);
            var predicao = predictionEngine.Predict(moto);

            // Usa Probability como base (0-1 → 0-100)
            var probabilidade = Math.Clamp(predicao.Probability * 100, 0, 100);
            var necessitaManutencao = predicao.PredictedLabel || probabilidade >= 50;

            var nivel = probabilidade switch
            {
                >= 80 => "CRÍTICO - Manutenção Urgente",
                >= 60 => "ALTO - Agendar Manutenção",
                >= 40 => "MÉDIO - Monitorar",
                >= 20 => "BAIXO - Em Boas Condições",
                _ => "ÓTIMO - Sem Necessidade"
            };

            return (probabilidade, necessitaManutencao, nivel);
        }
    }
}
