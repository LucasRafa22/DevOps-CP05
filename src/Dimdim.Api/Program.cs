using Dimdim.Application.Interfaces;
using Dimdim.Infrastructure.Data;
using Dimdim.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DimdimDb")
    ?? throw new InvalidOperationException("Connection string 'DimdimDb' não configurada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", async (AppDbContext db) =>
{
    var databaseOk = await db.Database.CanConnectAsync();
    return Results.Ok(new
    {
        status = databaseOk ? "Healthy" : "Unhealthy",
        api = "Healthy",
        database = databaseOk ? "Healthy" : "Unhealthy",
        timestamp = DateTime.UtcNow
    });
});

app.MapControllers();

app.Run();
