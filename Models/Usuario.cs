namespace SpotifyClone.API.Models;

public class Usuario
{
    public int Id { get; set; }

    public string GoogleId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Campos para usuarios de spotify premium

    public string? SpotifyUserId { get; set; }
    public string? SpotifyRefreshToken { get; set; }
    public string? SpotifyAccessToken { get; set; }
    public DateTime? SpotifyTokenExpiresAt { get; set; }
}
