using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using API_.Net.Controllers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace API.Net.Tests.IntegrationTests
{
    /// <summary>
    /// Factory customizada para configurar o ambiente de testes
    /// CORREÇÃO DEFINITIVA: Sobrescreve SetContentRoot para evitar erro
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private string? _contentRoot;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Encontra a raiz do projeto corretamente
            var projectDir = Directory.GetCurrentDirectory();
            
            Console.WriteLine($"[DEBUG] Current directory: {projectDir}");
            
            // Sobe até encontrar a raiz do projeto (onde está o arquivo .csproj)
            _contentRoot = projectDir;
            int maxLevels = 10;
            int currentLevel = 0;
            
            while (!File.Exists(Path.Combine(_contentRoot, "API .Net.csproj")) && 
                   Directory.GetParent(_contentRoot) != null &&
                   currentLevel < maxLevels)
            {
                _contentRoot = Directory.GetParent(_contentRoot)!.FullName;
                currentLevel++;
                Console.WriteLine($"[DEBUG] Checking: {_contentRoot}");
            }

            if (!File.Exists(Path.Combine(_contentRoot, "API .Net.csproj")))
            {
                throw new Exception($"Não foi possível encontrar 'API .Net.csproj'. Último diretório verificado: {_contentRoot}");
            }

            Console.WriteLine($"[DEBUG] Content root encontrado: {_contentRoot}");

            builder.UseContentRoot(_contentRoot);
            
            // Configurar appsettings de teste (se necessário)
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(Path.Combine(_contentRoot, "appsettings.json"), optional: true);
            });
            
            builder.ConfigureServices(services =>
            {
                // Aqui você pode configurar serviços de teste
                // Por exemplo, substituir o banco de dados por um in-memory
                // ou mockar serviços específicos
            });

            // CRÍTICO: Define o ambiente como Development
            builder.UseEnvironment("Development");
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Garante que o ContentRoot está definido antes de criar o host
            if (!string.IsNullOrEmpty(_contentRoot))
            {
                builder.UseContentRoot(_contentRoot);
            }
            
            return base.CreateHost(builder);
        }
    }

    /// <summary>
    /// Testes de integração usando WebApplicationFactory
    /// </summary>
    public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ApiIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task HealthCheck_DeveRetornar200_ComStatusHealthy()
        {
            var response = await _client.GetAsync("/health");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, "Health check deve retornar 200");
            content.Should().Contain("Healthy", "Status deve ser Healthy");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task HealthCheck_DeveRetornarJson_ComEstruturaCerta()
        {
            var response = await _client.GetAsync("/health");
            var content = await response.Content.ReadAsStringAsync();

            response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
            content.Should().Contain("status");
            content.Should().Contain("checks");
            content.Should().Contain("totalDuration");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task RootEndpoint_DeveRetornar200_ComInformacoesAPI()
        {
            var response = await _client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("Moto Tracking API");
            content.Should().Contain("version");
            content.Should().Contain("status");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Swagger_DeveEstarDisponivel()
        {
            var response = await _client.GetAsync("/swagger/index.html");
            response.StatusCode.Should().Be(HttpStatusCode.OK, "Swagger UI deve estar disponível");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task SwaggerJson_DeveRetornarDocumentacao()
        {
            var response = await _client.GetAsync("/swagger/v1/swagger.json");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("openapi");
            content.Should().Contain("Moto Tracking API");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
        {
            var loginRequest = new { username = "admin", password = "admin123" };

            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<JsonElement>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
            content.GetProperty("username").GetString().Should().Be("admin");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornar401()
        {
            var loginRequest = new { username = "usuario_invalido", password = "senha_errada" };
            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task PredicaoML_SemAutenticacao_DeveRetornar401()
        {
            var predicaoRequest = new
            {
                kilometragem = 50000,
                idadeMoto = 5,
                numeroRevisoesAtrasadas = 3
            };

            var response = await _client.PostAsJsonAsync("/api/v1/predicao/prever-manutencao", predicaoRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, "Endpoint de predição deve exigir autenticação");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task PredicaoML_ComAutenticacao_DeveRetornarPredicao()
        {
            var loginRequest = new { username = "admin", password = "admin123" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = loginContent.GetProperty("token").GetString();

            var authenticatedClient = _factory.CreateClient();
            authenticatedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var predicaoRequest = new
            {
                kilometragem = 50000,
                idadeMoto = 5,
                numeroRevisoesAtrasadas = 3
            };

            var response = await authenticatedClient.PostAsJsonAsync("/api/v1/predicao/prever-manutencao", predicaoRequest);
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("predicao");
            content.Should().Contain("recomendacao");
            content.Should().Contain("scoreProbabilidade");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task AuthValidate_ComTokenValido_DeveRetornar200()
        {
            var loginRequest = new { username = "admin", password = "admin123" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = loginContent.GetProperty("token").GetString();

            var authenticatedClient = _factory.CreateClient();
            authenticatedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await authenticatedClient.GetAsync("/api/v1/auth/validate");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task PredicaoML_ComDadosInvalidos_DeveRetornar400()
        {
            var loginRequest = new { username = "admin", password = "admin123" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var token = loginContent.GetProperty("token").GetString();

            var authenticatedClient = _factory.CreateClient();
            authenticatedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var predicaoInvalida = new
            {
                kilometragem = -1000,
                idadeMoto = -5,
                numeroRevisoesAtrasadas = -2
            };

            var response = await authenticatedClient.PostAsJsonAsync("/api/v1/predicao/prever-manutencao", predicaoInvalida);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task HealthCheck_DeveIncluirCheckDoBancoDeDados()
        {
            var response = await _client.GetAsync("/health");
            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("oracle", "Health check deve verificar o banco Oracle");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task API_DeveSuportarVersaoNaURL()
        {
            var responseV1 = await _client.GetAsync("/api/v1/auth/login");
            responseV1.StatusCode.Should().NotBe(HttpStatusCode.NotFound, "API deve suportar versionamento na URL");
        }
    }
}