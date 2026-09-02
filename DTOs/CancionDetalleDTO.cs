using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace SpotifyClone.API.DTOs;

public class CancionDetalleDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Album { get; set;} = string.Empty;
    public string Creador { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public int Duracion { get; set; }
    public string FechaLanzamiento { get; set; } = string.Empty;

    public string? PreviewUrl { get; set; }

}