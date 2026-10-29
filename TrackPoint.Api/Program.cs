using Microsoft.EntityFrameworkCore;
using TrackPoint.Api.Data;
using TrackPoint.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Respuestas de error estandarizadas
builder.Services.AddProblemDetails();

// Recupera la conexión almacenada en User Secrets
// dotnet user-secrets set "ConnectionStrings:TrackPoint" "server=localhost;port=3306;database=trackpoint;user=trackpoint_app;password=secretpsswd;"
var connectionString =
    builder.Configuration.GetConnectionString("TrackPoint")
    ?? throw new InvalidOperationException(
        "No se ha configurado ConnectionStrings:TrackPoint.");

builder.Services.AddDbContext<TrackPointDbContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddScoped<IVehicleService, VehicleService>();

var app = builder.Build();

// Captura excepciones inesperadas y genera respuestas ProblemDetails
app.UseExceptionHandler();

// Genera respuestas para errores HTTP que no tengan cuerpo
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // Esto permite testear la API con WebApplicationFactory en los tests de integración