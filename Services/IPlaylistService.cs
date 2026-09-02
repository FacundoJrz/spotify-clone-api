using SpotifyClone.API.DTOs;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Services;

public interface IPlaylistService
{
    Task<List<PlaylistDto>> ObtenerPlaylistsUsuarioAsync(int usuarioId);
    Task<PlaylistDto?> ObtenerPorIdAsync(int id);
    Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto);
    Task<bool> AgregarContenidoAsync(int playlistId, AgregarContenidoPlaylistDto dto);
    Task<bool> EliminarContenidoAsync(int playlistId, int Id); //el 2do parametro int Id es de playlist contenido
    Task<bool> EliminarPlaylistAsync(int playlistId);
}