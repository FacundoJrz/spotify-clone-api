namespace SpotifyClone.API.models;

public class Usuario
{
    public int Id { get; set; }

    public string GoogleId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}