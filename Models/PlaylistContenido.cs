namespace SpotifyClone.API.Models;

public class PlaylistContenido
{
    public int Id { get; set; }
    public int PlaylistId { get; set; } //FK de la playlist local
    public string SpotifyId { get; set;} = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Creador { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public int DuracionMs { get; set; }



    public virtual Playlist Playlist { get; set; } = null!; //propiedad de navegación

}