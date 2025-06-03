using Microsoft.EntityFrameworkCore;
using Coretallerauto.Server.Data;
using Coretallerauto.Server.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- CORS --- Configura la política de CORS para permitir Angular en puerto 4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // <-- ¡IMPORTANTE! si usas cookies o autenticación
    });
});

// --- JSON Options (para evitar ciclos y mejorar JSON) ---
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;
});

// --- DB Context y Servicios ---
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<AsignacionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Middleware ---
// ¡ACTIVA CORS antes de MVC!
app.UseCors("AllowAngularApp");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// --- Fallback para Angular ---
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

// --- Migraciones automáticas ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();



