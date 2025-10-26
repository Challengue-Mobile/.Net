using API_.Net.Data;
using API_.Net.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// ==================== CONTROLLERS ====================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ==================== DATABASE ====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("API .Net")));

// ==================== VERSIONAMENTO DA API (10 PONTOS) ====================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ==================== AUTENTICAÇÃO JWT (25 PONTOS) ====================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "ChaveSecretaParaJWT_MinimoDe32Caracteres!@#$%");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Em produção, mude para true
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "MotoTrackingAPI",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "MotoTrackingClient",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ==================== HEALTH CHECKS (10 PONTOS) ====================
builder.Services.AddHealthChecks()
    .AddOracle(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? "",
        name: "oracle-database",
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db", "oracle", "database" })
    .AddCheck("api-health", () => 
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API está funcionando corretamente"));

// ==================== ML.NET SERVICE (25 PONTOS) ====================
builder.Services.AddSingleton<MLService>();

// ==================== SWAGGER/OPENAPI ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Moto Tracking API",
        Version = "v1.0",
        Description = @"API REST completa para gerenciamento de motos, localização e rastreamento.
        
**Funcionalidades:**
- ✅ Health Checks em /health
- ✅ Versionamento de API (v1.0)
- ✅ Autenticação JWT Bearer
- ✅ Machine Learning com ML.NET
- ✅ CRUD completo de motos, clientes, funcionários
- ✅ Rastreamento de localização em tempo real
        
**Autenticação:**
1. Faça login em POST /api/v1/auth/login
2. Use o token retornado no header: `Authorization: Bearer {token}`",
        Contact = new OpenApiContact
        {
            Name = "Challengue Mobile Team",
            Email = "suporte@mototracking.com"
        }
    });

    // Configuração de segurança JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando Bearer scheme.
        
**Como usar:** 
1. Faça login no endpoint /api/v1/auth/login
2. Copie o token retornado
3. Clique em 'Authorize' e digite: Bearer {seu-token}
4. Clique em 'Authorize' novamente para salvar

Exemplo: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    // Incluir XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
        Console.WriteLine($"✅ Documentação XML carregada: {xmlPath}");
    }
    else
    {
        Console.WriteLine($"⚠️ Arquivo XML não encontrado: {xmlPath}");
        Console.WriteLine("   Para gerar: Adicione <GenerateDocumentationFile>true</GenerateDocumentationFile> no .csproj");
    }

    // Configurações adicionais
    c.EnableAnnotations();
});

var app = builder.Build();

// ==================== PIPELINE DE REQUISIÇÕES ====================

// Swagger disponível em todos os ambientes (desenvolvimento e produção)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto Tracking API v1.0");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Moto Tracking API - Documentação";
    c.DefaultModelsExpandDepth(2);
    c.DisplayRequestDuration();
});

// ==================== HEALTH CHECK ENDPOINT (10 PONTOS) ====================
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description ?? "N/A",
                duration = $"{e.Value.Duration.TotalMilliseconds}ms",
                error = e.Value.Exception?.Message
            }),
            totalDuration = $"{report.TotalDuration.TotalMilliseconds}ms"
        }, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        await context.Response.WriteAsync(result);
    }
});

// Endpoint raiz com informações da API
app.MapGet("/", () => Results.Ok(new
{
    api = "Moto Tracking API",
    version = "v1.0",
    status = "online",
    endpoints = new
    {
        documentacao = "/swagger",
        healthCheck = "/health",
        login = "/api/v1/auth/login",
        predicaoML = "/api/v1/predicao/prever-manutencao"
    },
    features = new[]
    {
        "✅ Health Checks",
        "✅ API Versioning",
        "✅ JWT Authentication",
        "✅ ML.NET Integration",
        "✅ Full CRUD Operations",
        "✅ Real-time Tracking"
    }
}))
.WithName("GetApiInfo")
.WithTags("Info")
.Produces(200);

app.UseHttpsRedirection();

// ⚠️ IMPORTANTE: Ordem correta
app.UseAuthentication(); // PRIMEIRO: Autenticação
app.UseAuthorization();  // DEPOIS: Autorização

app.MapControllers();

// ==================== LOGGING DE INICIALIZAÇÃO ====================
Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║           🏍️  MOTO TRACKING API INICIADA                    ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
Console.WriteLine();
Console.WriteLine("✅ Health Checks disponível em:     /health");
Console.WriteLine("✅ Swagger UI disponível em:         /swagger");
Console.WriteLine("✅ API versão:                       v1.0");
Console.WriteLine("✅ Autenticação:                     JWT Bearer");
Console.WriteLine("✅ Machine Learning:                 ML.NET ativo");
Console.WriteLine();
Console.WriteLine("🔐 Para testar endpoints protegidos:");
Console.WriteLine("   1. POST /api/v1/auth/login (user: admin, pass: admin123)");
Console.WriteLine("   2. Copie o token e use: Authorization: Bearer {token}");
Console.WriteLine();
Console.WriteLine("═══════════════════════════════════════════════════════════════");

app.Run();

// Tornar Program acessível para testes de integração
public partial class Program { }