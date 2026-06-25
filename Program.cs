using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SpotifyClone.API.Data;
using SpotifyClone.API.Services;


var builder = WebApplication.CreateBuilder(args);
//cadena de conexión en appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//inyeccion del applicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Inyección del soporte para Controllers
builder.Services.AddControllers();
builder.Services.AddHttpClient<ISpotifyService, SpotifyService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Linea para mapear con Scalar (librería para los endpoints)
}

app.UseHttpsRedirection();


// Activar el mapeo de los controladores
app.MapControllers();
app.Run();
