using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace API_.Net.Examples
{
    /// <summary>
    /// Exemplo de resposta para erros de validação
    /// </summary>
    public class ValidationErrorResponseExample : IExamplesProvider<ValidationProblemDetails>
    {
        public ValidationProblemDetails GetExamples()
        {
            var problemDetails = new ValidationProblemDetails
            {
                Title = "Um ou mais erros de validação ocorreram",
                Status = 400,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            problemDetails.Errors.Add("PLACA", new[] { "A placa é obrigatória", "A placa deve estar no formato XXX0000" });
            problemDetails.Errors.Add("ID_MODELO_MOTO", new[] { "O modelo da moto é obrigatório" });

            return problemDetails;
        }
    }

    /// <summary>
    /// Exemplo de resposta para erro de não encontrado
    /// </summary>
    public class NotFoundResponseExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Recurso não encontrado",
                Status = 404,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Detail = "O item solicitado não foi encontrado"
            };
        }
    }
}