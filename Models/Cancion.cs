namespace SpotifyClone.API.Models;

public class Cancion : ContenidoAudio
{
    public string Artista { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
}