using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottothTracking.Data;

namespace MottothTracking;

public partial class Program { }

public static class AppBootstrap
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers + Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // String de conexão (vem do ACI via env "ConnectionStrings__OracleConnection")
        var oracleConn = builder.Configuration.GetConnectionString("OracleConnection") ?? "";

        // EF Core + Oracle
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            // Requer o provider Oracle EF Core instalado no projeto.
            // Ex.: dotnet add package Oracle.EntityFrameworkCore
            opt.UseOracle(oracleConn);
        });

        var app = builder.Build();

        // Swagger em prod (ok pra sprint)
        app.UseSwagger();
        app.UseSwaggerUI();

        // Em container é HTTP:8080; não forçar HTTPS
        // app.UseHttpsRedirection();

        // Health
        app.MapGet("/", () => Results.Ok("MottothTracking API OK"))
            .WithTags("Health");

        // Ping simples
        app.MapGet("/api/ping", () =>
                Results.Json(new { pong = true, when = DateTime.UtcNow }))
            .WithTags("Health");

        // Controllers (CRUD)
        app.MapControllers();

        app.Run();
    }
}