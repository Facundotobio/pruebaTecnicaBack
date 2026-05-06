using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS (builder.Services)
// ==================== CORS CONFIGURATION ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "https://localhost:3000",
            "http://127.0.0.1:3000",
            "https://prueba-tecnica-front-sigma.vercel.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// ==================== DATABASE ====================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("La cadena de conexión 'DefaultConnection' no fue encontrada.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ==================== CONTROLLERS + JSON ====================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. CONSTRUCCIÓN DE LA APP
var app = builder.Build();

// 3. MIGRACIONES AUTOMÁTICAS
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Migraciones aplicadas con éxito.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
    }
}

// 4. CONFIGURACIÓN DEL MIDDLEWARE (app.Use...)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// 5. ARRANCAR LA APP
app.Run();