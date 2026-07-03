namespace SpotifyClone.API.DTOs;

public class PodcastDetalleDto
{
    public string Id { get; set;} = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set;} = string.Empty;
    public string Creador { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public int TotalEpisodios { get; set; }

}