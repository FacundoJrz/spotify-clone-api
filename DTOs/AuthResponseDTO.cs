namespace SpotifyClone.API.DTOs;

public class AuthResponseDto
{
    public required string Token { get; set; }
    
    public required UsuarioDto Usuario { get; set; }
}