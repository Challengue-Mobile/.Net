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
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("API .Net"))); 

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Moto Tracking API", 
        Version = "v1",
        Description = "API REST para gerenciamento de motos, localização e rastreamento",
        Contact = new OpenApiContact 
        {
            Name = "Seu Nome",
            Email = "seu.email@example.com",
            Url = new Uri("https://github.com/seu-usuario/moto-tracking-api")
        },
        License = new OpenApiLicense 
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
 
    c.ExampleFilters();
    
 
    c.EnableAnnotations(); 
    c.DescribeAllParametersInCamelCase();
    
   
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                }
            },
            new string[] {}
        }
    });
});

// Registra exemplos de requisição/resposta para o Swagger
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();


// Configuração do the HTTP request pipelineapp.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto Tracking API v1");
        //c.RoutePrefix = "swagger"; // Faz o Swagger UI ser a página inicial
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DefaultModelsExpandDepth(-1); // Oculta esquemas por padrão
        c.EnableFilter(); // Habilita filtro para endpoints
        c.EnableDeepLinking(); // Permite links diretos para operações específicas
        
        // Personaliza título
        c.DocumentTitle = "Moto Tracking API - Documentação";
    });

app.MapFallback(async context => 
{
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync("API está funcionando! Acesse /swagger para ver a documentação.");
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();