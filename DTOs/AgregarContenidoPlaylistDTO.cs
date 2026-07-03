namespace SpotifyClone.API.DTOs;

public class AgregarContenidoPlaylistDto
{
    public string SpotifyId { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Para determinar si es canción o podcast
}