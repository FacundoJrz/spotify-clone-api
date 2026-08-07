using SpotifyClone.API.DTOs;

namespace SpotifyClone.API.Services;

public interface IPlaylistService
{
    Task<List<PlaylistDto>> ObtenerPlaylistsUsuarioAsync(int usuarioId);
    Task<PlaylistDto?> ObtenerPorIdAsync(int id);
    Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto);
    Task<bool> AgregarContenidoAsync(int playlistId, AgregarContenidoPlaylistDto dto);
    Task<bool> EliminarPlaylistAsync(int playlistId);
}