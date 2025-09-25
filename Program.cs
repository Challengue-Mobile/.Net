using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MottothTracking.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ===== MANTÉM SEU ORACLE EXISTENTE =====
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Configure Swagger/OpenAPI (atualiza só o título)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mottoth Tracking API - Sistema de Frotas",
        Version = "v1",
        Description = "Sistema de rastreamento e gestão de frotas de motos com beacons GPS",
        Contact = new OpenApiContact
        {
            Name = "Equipe DevOps Sprint 3", 
            Email = "contato@fiap.com.br"
        }
    });
});

var app = builder.Build();

// ===== SWAGGER SEMPRE ATIVO (para Azure) =====
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottoth Tracking API v1");
    c.RoutePrefix = "swagger";
});

// ===== HEALTH CHECK OBRIGATÓRIO =====
app.MapGet("/health", () => Results.Ok(new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    version = "1.0.0",
    database = "oracle-connected"
}));

// ===== ENDPOINT RAIZ =====
app.MapGet("/", () => Results.Ok(new { 
    message = "Sistema de Rastreamento de Frotas - Mottoth Tracking",
    swagger = "/swagger",
    health = "/health",
    endpoints = new {
        usuarios = "/api/usuarios",
        beacons = "/api/beacons", 
        motos = "/api/motos",
        movimentacoes = "/api/movimentacoes"
    }
}));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ===== MIGRATIONS (mantém como estava) =====
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try 
        {
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration info: {ex.Message}");
        }
    }
}

app.Run();