using SpotifyClone.API.DTOs;

namespace SpotifyClone.API.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<ContenidoAudioDto>> BuscarContenidoAsync(string query);
        Task<CancionDetalleDto?> ObtenerCancionPorIdAsync(string id);
        Task<PodcastDetalleDto?> ObtenerPodcastPorIdAsync(string id);

    }
}