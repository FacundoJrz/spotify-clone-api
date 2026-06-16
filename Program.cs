using Microsoft.EntityFrameworkCore;
using SpotifyClone.API.Data;

var builder = WebApplication.CreateBuilder(args);
//cadena de conexión en appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//inyeccion del applicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();
