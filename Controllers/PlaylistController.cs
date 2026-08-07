using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.API.DTOs;
using SpotifyClone.API.Models;
using SpotifyClone.API.Services;

namespace SpotifyClone.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class PlaylistController : ControllerBase
{
    
    private readonly IPlaylistService _playlistService;

    public PlaylistController (IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }
    [AllowAnonymous]
    [HttpGet("usuario/{UsuarioId}")]

    public async Task<IActionResult> ObtenerPlaylistsUsuarioAsync(int usuarioId)
    {
        var playlists = await _playlistService.ObtenerPlaylistsUsuarioAsync(usuarioId);
        if (playlists == null || playlists.Count == 0) return NotFound("No hay playlists para este usuario");
        return Ok(playlists);
    }

    [HttpPost("usuario/{UsuarioId}")]

    public async Task<IActionResult> CrearPlaylist(int usuarioId, [FromBody] CrearPlaylistDto dto)
    {
        var resultado = await _playlistService.CrearAsync(usuarioId, dto);
        return CreatedAtAction(nameof(ObtenerPlaylist), new {id = resultado.Id}, resultado);
    }
    
    [HttpGet("{playlistId}")]

    public async Task<IActionResult> ObtenerPlaylist(int playlistId)
    {
        var playlist = await _playlistService.ObtenerPorIdAsync(playlistId);
        if(playlist == null) return NotFound("La playlist solicitada no existe");
        return Ok(playlist);
    }

    [HttpPost("{playlistId}/contenido")]

    public async Task<IActionResult> AgregarContenido(int playlistId, [FromBody]AgregarContenidoPlaylistDto dto)
    {
        var exito = await _playlistService.AgregarContenidoAsync(playlistId, dto);
        if (!exito)
        {
            return BadRequest("No se pudo agregar el contenido a la playlist");
        } return Ok(new {mensaje = "El contenido se agregó con exito"});

    }

    [HttpDelete("{playlistId}")]

    public async Task<IActionResult>EliminarPlaylist(int playlistId)
    {
        var exito = await _playlistService.EliminarPlaylistAsync(playlistId);
        if (!exito) return NotFound("No se encontró la playlist solicitada");
        return NoContent();
    }
}