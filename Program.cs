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

        // Connection string vem de:
        // - env var: ConnectionStrings__Default
        // - ou appsettings.json (fallback)
        var sqlConn = builder.Configuration.GetConnectionString("Default") 
                      ?? throw new InvalidOperationException("ConnectionStrings:Default não configurada.");

        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(sqlConn, sql =>
            {
                // Resiliência em caso de falha de conexão
                sql.EnableRetryOnFailure(maxRetryCount: 5);
            });
        });

        var app = builder.Build();

        // Swagger em qualquer ambiente (ok para sprint)
        app.UseSwagger();
        app.UseSwaggerUI();

        // Health
        app.MapGet("/", () => Results.Ok("MottothTracking API OK")).WithTags("Health");
        app.MapGet("/api/ping", () => Results.Json(new { pong = true, when = DateTime.UtcNow }))
            .WithTags("Health");

        // Controllers
        app.MapControllers();

        // (Opcional) aplica migrations no startup
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN] Migrations não aplicadas: {ex.Message}");
        }

        app.Run();
    }
}
