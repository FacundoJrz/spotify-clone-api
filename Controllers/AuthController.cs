using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Services;

namespace SpotifyClone.API.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController (IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("google")]

    public async Task<IActionResult> LoginWithGoogleAsync([FromBody] GoogleLoginDto dto)
    {
        try
        {
            var response = await _authService.LoginWithGoogleAsync(dto);
            return Ok(response);

        }catch(UnauthorizedAccessException ex)
        {
            return Unauthorized(new{mensaje = ex.Message});
        }
        catch(Exception ex)
        {
            return StatusCode(500, new{mensaje = "Ocurrio un error inesperado durante la autenticación", detalle = ex.Message});
        }
    }
}