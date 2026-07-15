using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpotifyClone.API.Data;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration  _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginWithGoogleAsync(GoogleLoginDto dto)
    {
        var googleClientId = _configuration["Authentication:Google:ClientId"];    
        var validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] {googleClientId} 
        };

        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, validationSettings);
        }
        catch(InvalidJwtException ex)
        {
            throw new UnauthorizedAccessException("El token de Google no es Valido o expiró", ex);
        }

        var usuario = await _context.Set<Usuario>()
        .FirstOrDefaultAsync(u=> u.GoogleId == payload.Subject);
        
        if (usuario == null)
        {
            usuario = new Usuario
            {
                GoogleId = payload.Subject,
                Nombre = payload.Name,
                Email = payload.Email
            };
            _context.Set<Usuario>().Add(usuario);
            await _context.SaveChangesAsync();
        } 

        var jwtPropio = GenerarJwtPropio(usuario);

        return new AuthResponseDto
        {
            Token = jwtPropio,
            Usuario = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                GoogleId = usuario.GoogleId
            }
        };
    }

    private string GenerarJwtPropio(Usuario usuario)
    {
        var secretKey = _configuration["Authentication:Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt Key no configurada");
        var issuer = _configuration["Authentication:Jwt:Issuer"];
        var audience = _configuration["Authentication:Jwt:Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim("GoogleId", usuario.GoogleId)
        };

        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),   
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.CreateToken(token);

        return tokenHandler.WriteToken(stringToken);
        

    }

}