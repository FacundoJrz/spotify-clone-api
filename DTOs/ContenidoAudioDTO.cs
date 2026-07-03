namespace SpotifyClone.API.DTOs;

public class ContenidoAudioDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Creador { get; set; } = string.Empty; //Este varía en artísta si es canción o programa en caso de ser podcast
    public string ImagenUrl { get; set; } = string.Empty;
    public int DuracionMs { get; set; }
    public string Tipo { get; set; } = string.Empty; // Tipo de archivo (canción o podcast)



}