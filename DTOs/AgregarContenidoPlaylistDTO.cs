namespace SpotifyClone.API.DTOs;

public class AgregarContenidoPlaylistDto
{
    public int PlaylistId { get; set; }
    public string SpotifyId { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Para determinar si es canción o podcast
    public string Nombre { get; set; } = string.Empty;
    public string Creador { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public int DuracionMs { get; set; }


}