using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SpotifyClone.API.Data;
using SpotifyClone.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
//cadena de conexión en appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//inyeccion del applicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Inyección del soporte para Controllers
builder.Services.AddControllers();
// Inyección del soporte para Services
builder.Services.AddHttpClient<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Extraer las variables de entorno de Jwt
var jwtKey = builder.Configuration["Authentication:Jwt:Key"] 
    ?? throw new InvalidOperationException("La clave secreta de JWT no está configurada.");
var jwtIssuer = builder.Configuration["Authentication:Jwt:Issuer"];
var jwtAudience = builder.Configuration["Authentication:Jwt:Audience"];

//Configuración el sistema de Autenticación global de la API
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                
        ValidateAudience = true,             
        ValidateLifetime = true,              
        ValidateIssuerSigningKey = true,     

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        
        ClockSkew = TimeSpan.Zero 
    };
});
builder.Services.AddAuthorization();



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

app.UseAuthentication();
app.UseAuthorization(); 


// Activar el mapeo de los controladores
app.MapControllers();
app.Run();
