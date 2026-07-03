using SpotifyClone.API.DTOs;

namespace SpotifyClone.API.Services;

public interface IPlaylistService
{
    Task<PlaylistDto?> ObtenerPorIdAsync(int id);
    Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto);
    Task<bool> AgregarContenidoAsync(int playlistId, AgregarContenidoPlaylistDto dto);
}