using SpotifyClone.API.DTOs;
using SpotifyClone.API.Models;

namespace SpotifyClone.API.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();
    Task<UsuarioDto?>ObtenerPorIdAsync(int id);
    Task<UsuarioDto>CrearAsync(UsuarioRequestDto dto);
    Task<bool>ActualizarAsync(int id, UsuarioRequestDto dto);
    Task<bool>EliminarAsync(int id);
}