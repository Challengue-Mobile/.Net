using Microsoft.ML;
using Microsoft.ML.Data;

namespace API_.Net.Services
{
    /// <summary>
    /// Classe de entrada de dados para o modelo ML
    /// </summary>
    public class MotoData
    {
        [LoadColumn(0)]
        public float Kilometragem { get; set; }

        [LoadColumn(1)]
        public float IdadeMoto { get; set; }

        [LoadColumn(2)]
        public float NumeroRevisoesAtrasadas { get; set; }
    }

    /// <summary>
    /// Classe de saída da predição
    /// </summary>
    public class MotoPredicao
    {
        [ColumnName("Score")]
        public float Probabilidade { get; set; }

        [ColumnName("PredictedLabel")]
        public bool NecessitaManutencao { get; set; }
    }

    /// <summary>
    /// Serviço de Machine Learning para predição de manutenção de motos
    /// </summary>
    public class MLService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;
        private readonly string _modelPath = "MLModels/modelo_manutencao.zip";

        public MLService()
        {
            _mlContext = new MLContext(seed: 0);
            
            // Tenta carregar o modelo, se não existir usa lógica heurística
            try
            {
                if (File.Exists(_modelPath))
                {
                    _model = _mlContext.Model.Load(_modelPath, out var _);
                    Console.WriteLine("✅ Modelo ML carregado com sucesso!");
                }
                else
                {
                    Console.WriteLine("⚠️ Modelo ML não encontrado. Usando lógica heurística.");
                    _model = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erro ao carregar modelo ML: {ex.Message}");
                Console.WriteLine("   Usando lógica heurística para predições.");
                _model = null;
            }
        }

        /// <summary>
        /// Faz a predição com detalhes
        /// </summary>
        public (float probabilidade, bool necessita, string nivel) PreverComDetalhes(MotoData dados)
        {
            float probabilidade;
            bool necessita;

            if (_model != null)
            {
                // Usar modelo ML treinado
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<MotoData, MotoPredicao>(_model);
                var predicao = predictionEngine.Predict(dados);
                probabilidade = predicao.Probabilidade * 100;
                necessita = predicao.NecessitaManutencao;
            }
            else
            {
                // Usar lógica heurística (para quando o modelo não está disponível)
                probabilidade = CalcularProbabilidadeHeuristica(dados);
                necessita = probabilidade >= 50;
            }

            // Garantir que está no intervalo [0, 100]
            probabilidade = Math.Clamp(probabilidade, 0, 100);

            string nivel = ObterNivelRisco(probabilidade);

            return (probabilidade, necessita, nivel);
        }

        /// <summary>
        /// Calcula probabilidade usando lógica heurística baseada em regras
        /// </summary>
        private float CalcularProbabilidadeHeuristica(MotoData dados)
        {
            float score = 0;

            // Peso para quilometragem (0-40 pontos)
            if (dados.Kilometragem >= 80000)
                score += 40;
            else if (dados.Kilometragem >= 50000)
                score += 30;
            else if (dados.Kilometragem >= 25000)
                score += 20;
            else if (dados.Kilometragem >= 10000)
                score += 10;
            else
                score += 5;

            // Peso para idade da moto (0-35 pontos)
            if (dados.IdadeMoto >= 7)
                score += 35;
            else if (dados.IdadeMoto >= 5)
                score += 28;
            else if (dados.IdadeMoto >= 3)
                score += 18;
            else if (dados.IdadeMoto >= 1)
                score += 8;
            else
                score += 3;

            // Peso para revisões atrasadas (0-25 pontos)
            if (dados.NumeroRevisoesAtrasadas >= 5)
                score += 25;
            else if (dados.NumeroRevisoesAtrasadas >= 3)
                score += 20;
            else if (dados.NumeroRevisoesAtrasadas >= 2)
                score += 12;
            else if (dados.NumeroRevisoesAtrasadas >= 1)
                score += 6;
            else
                score += 0;

            // Score máximo é 100 (40 + 35 + 25)
            return Math.Clamp(score, 0, 100);
        }

        /// <summary>
        /// Determina o nível de risco baseado na probabilidade
        /// </summary>
        private string ObterNivelRisco(float probabilidade)
        {
            return probabilidade switch
            {
                >= 80 => "🔴 CRÍTICO - Manutenção urgente necessária!",
                >= 60 => "🟠 ALTO - Agende manutenção em breve",
                >= 40 => "🟡 MÉDIO - Fique atento",
                >= 20 => "🟢 BAIXO - Boas condições",
                _ => "✅ ÓTIMO - Moto em excelente estado"
            };
        }

        /// <summary>
        /// Faz a predição simples (retorna apenas probabilidade)
        /// </summary>
        public float Prever(MotoData dados)
        {
            var (probabilidade, _, _) = PreverComDetalhes(dados);
            return probabilidade;
        }

        /// <summary>
        /// Treina um novo modelo (método auxiliar para criar o modelo inicial)
        /// </summary>
        public void TreinarModelo()
        {
            Console.WriteLine("🔄 Iniciando treinamento do modelo...");

            // Dados de exemplo para treinamento
            var dadosTreinamento = new List<DadosTreinamento>
            {
                // Motos que NÃO precisam de manutenção
                new() { Kilometragem = 1000, IdadeMoto = 0.5f, NumeroRevisoesAtrasadas = 0, NecessitaManutencao = false },
                new() { Kilometragem = 5000, IdadeMoto = 1, NumeroRevisoesAtrasadas = 0, NecessitaManutencao = false },
                new() { Kilometragem = 8000, IdadeMoto = 1.5f, NumeroRevisoesAtrasadas = 0, NecessitaManutencao = false },
                new() { Kilometragem = 15000, IdadeMoto = 2, NumeroRevisoesAtrasadas = 1, NecessitaManutencao = false },
                new() { Kilometragem = 20000, IdadeMoto = 2.5f, NumeroRevisoesAtrasadas = 1, NecessitaManutencao = false },

                // Motos que PRECISAM de manutenção
                new() { Kilometragem = 50000, IdadeMoto = 5, NumeroRevisoesAtrasadas = 3, NecessitaManutencao = true },
                new() { Kilometragem = 80000, IdadeMoto = 7, NumeroRevisoesAtrasadas = 5, NecessitaManutencao = true },
                new() { Kilometragem = 60000, IdadeMoto = 6, NumeroRevisoesAtrasadas = 4, NecessitaManutencao = true },
                new() { Kilometragem = 100000, IdadeMoto = 10, NumeroRevisoesAtrasadas = 8, NecessitaManutencao = true },
                new() { Kilometragem = 45000, IdadeMoto = 4, NumeroRevisoesAtrasadas = 3, NecessitaManutencao = true },

                // Casos intermediários
                new() { Kilometragem = 25000, IdadeMoto = 3, NumeroRevisoesAtrasadas = 2, NecessitaManutencao = true },
                new() { Kilometragem = 30000, IdadeMoto = 3.5f, NumeroRevisoesAtrasadas = 2, NecessitaManutencao = true },
                new() { Kilometragem = 18000, IdadeMoto = 2, NumeroRevisoesAtrasadas = 1, NecessitaManutencao = false },
                new() { Kilometragem = 22000, IdadeMoto = 2.8f, NumeroRevisoesAtrasadas = 1, NecessitaManutencao = false },
            };

            var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

            // Pipeline de treinamento
            var pipeline = _mlContext.Transforms.Concatenate("Features", 
                    nameof(DadosTreinamento.Kilometragem),
                    nameof(DadosTreinamento.IdadeMoto),
                    nameof(DadosTreinamento.NumeroRevisoesAtrasadas))
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(
                    labelColumnName: nameof(DadosTreinamento.NecessitaManutencao),
                    featureColumnName: "Features",
                    numberOfLeaves: 20,
                    numberOfTrees: 100,
                    minimumExampleCountPerLeaf: 1,
                    learningRate: 0.2));

            // Treinar o modelo
            _model = pipeline.Fit(dataView);

            // Salvar o modelo
            Directory.CreateDirectory(Path.GetDirectoryName(_modelPath)!);
            _mlContext.Model.Save(_model, dataView.Schema, _modelPath);

            Console.WriteLine($"✅ Modelo treinado e salvo em: {_modelPath}");
        }

        /// <summary>
        /// Classe auxiliar para dados de treinamento
        /// </summary>
        private class DadosTreinamento
        {
            public float Kilometragem { get; set; }
            public float IdadeMoto { get; set; }
            public float NumeroRevisoesAtrasadas { get; set; }
            public bool NecessitaManutencao { get; set; }
        }
    }
}