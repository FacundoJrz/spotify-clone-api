namespace SpotifyClone.API.models;

public class Playlist
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // --- Relación para entity Framework ---
    // FK
    public int UsuarioId { get; set; }

    // propiedad de Navegación
    public Usuario? Usuario { get; set; }
public List<ContenidoAudio> Contenidos { get; set; } = new ();


}