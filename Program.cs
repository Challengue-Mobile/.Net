using API_.Net.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;

var builder = WebApplication.CreateBuilder(args);

// Adiciona controllers com configuração JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configura o DbContext com Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("API .Net"))); 

// Configuração simplificada do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Moto Tracking API", 
        Version = "v1",
        Description = "API REST para gerenciamento de motos, localização e rastreamento"
    });
    
    // Configuração do XML para documentação
    try
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
            Console.WriteLine($"Arquivo XML de documentação carregado com sucesso: {xmlPath}");
        }
        else
        {
            Console.WriteLine($"ATENÇÃO: Arquivo XML não encontrado em: {xmlPath}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERRO ao configurar XML para Swagger: {ex.Message}");
    }
});

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
// Removendo a verificação de ambiente para garantir que o Swagger funcione em todos os ambientes
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        Console.WriteLine($"Gerando Swagger JSON para {httpReq.Path}");
    });
});

app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto Tracking API v1");
    // Deixando o Swagger UI disponível no caminho padrão
    c.RoutePrefix = "swagger";
});

// Adiciona um endpoint de fallback para diagnóstico
app.Map("/", () => "API está funcionando! Acesse /swagger para ver a documentação.");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Aplicação inicializada. Swagger disponível em  aqui /swagger");

await app.RunAsync();