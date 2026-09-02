using Microsoft.AspNetCore.Mvc;
using SpotifyClone.API.Services;

namespace SpotifyClone.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SpotifyAuthController : ControllerBase
{
    private readonly ISpotifyAuthService _spotifyAuthService;
    private readonly IConfiguration _configuration;

    public SpotifyAuthController(ISpotifyAuthService spotifyAuthService, IConfiguration configuration)
    {
        _spotifyAuthService = spotifyAuthService;
        _configuration = configuration;
    }
    [HttpGet("login")]
    public IActionResult Login()
    {
        var url = _spotifyAuthService.ObtenerUrlAutorización();
        return Redirect(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string? error)
    {

        if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
        {
            return BadRequest(new { mensaje = "Atorización denegada por el usuario o error de spotify", error });
        }


        var tokens = await _spotifyAuthService.IntercambiarCodigoPorTokensAsync(code);
        if (tokens == null)
        {
            return BadRequest(new { mensaje = "No se pudieron obtener los tokens de acceso" });
        }

        var frontendUrl = _configuration["Authentication:FrontendUrl"] ?? "https://localhost:5173";

        return Redirect($"{frontendUrl}/spotify-callback?access_token={tokens.AccessToken}&refresh_token={tokens.RefreshToken}&expires_in={tokens.ExpiresIn}");

    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(new { mensaje = "El refresh_token es requerido" });
        }
        var tokens = await _spotifyAuthService.RefrescarTokenAsync(refreshToken);
        if (tokens == null)
        {
            return Unauthorized(new { mensaje = "Token inválido o expirado" });
        }
        return Ok(tokens);
    }
}