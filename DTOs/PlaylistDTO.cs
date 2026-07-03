namespace SpotifyClone.API.DTOs;

public class PlaylistDto
{
    public int Id { get; set; }
    public string NombrePlaylist { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }

    public List<ContenidoAudioDto> Elementos { get; set; } = new List<ContenidoAudioDto>();


}