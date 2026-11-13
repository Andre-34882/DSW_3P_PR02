using System.Text.Json.Serialization;
using DSW_3P_PR02.Data;
using DSW_3P_PR02.Middleware;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddScoped<IEventoTecnologicoService, EventoTecnologicoService>();

var persistenceProvider = builder.Configuration["Persistence:Provider"] ?? "SqlServer";
if (persistenceProvider.Equals("Json", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton<IEventoTecnologicoRepository, EventoTecnologicoJsonRepository>();
}
else if (persistenceProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection")
        ?? throw new InvalidOperationException(
            "Falta configurar ConnectionStrings:SqlServerConnection.");

    builder.Services.AddDbContext<EventosDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddScoped<IEventoTecnologicoRepository, EventoTecnologicoSqlRepository>();
}
else
{
    throw new InvalidOperationException(
        $"El proveedor de persistencia '{persistenceProvider}' no es válido. Use Json o SqlServer.");
}

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Gestor de Eventos Tecnológicos v1"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
