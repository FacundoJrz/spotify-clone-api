namespace SpotifyClone.API.models;

public abstract class ContenidoAudio
{
    public int Id { get; set;} 
    public string SpotifyId { get; set;} = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int DuracionSegundos { get; set; }
    public List<Playlist> Playlists { get; set; } = new ();

} 