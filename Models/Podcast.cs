namespace SpotifyClone.API.Models;

public class Podcast : ContenidoAudio
{
    public string Anfitrion { get; set; } = string.Empty;
    public string DescripcionEpisodio { get; set; } = string.Empty;
    public string Temporada { get; set; } = string.Empty;
}