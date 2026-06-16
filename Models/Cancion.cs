namespace SpotifyClone.API.models;

public class Cancion : ContenidoAudio
{
    public string Artista { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
}